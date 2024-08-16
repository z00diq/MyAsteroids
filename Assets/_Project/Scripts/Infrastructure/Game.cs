using Assets.Infrastructure;
using Assets.Models;
using Assets.Views;
using System;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class Game:MonoBehaviour
    {
        private static Game _instance = null;
        public static Game Instance => _instance;

        [Header("Asteroid Settings")]
        [SerializeField] AsteroidConfiguration _asteroidsConfig;
        [SerializeField] ParticleAsteroidConfiguration _smallAsteroidConfig;

        [Header("UFO Settings")]
        [SerializeField] private UFOConfiguration _ufoConfig;

        [Header("Ship Settings")]
        [SerializeField] private ShipConfigure _shipConfig;

        [Header("Ship Fire Settings")]
        [SerializeField] private LaserFireConfigure _laserConfig;
        [SerializeField] private BulletsFireConfigure _bulletsConfig;
        
        [SerializeField] private ShipUI _shipUI;

        private GameLoop _gameLoop;
        private SceneSecretary _sceneSecretary;

        public Vector2 ScreenBounds { get; private set; }

        private void Awake()
        {
            _instance = FindObjectOfType<Game>();
            _gameLoop = new GameLoop();
            _sceneSecretary = new SceneSecretary();
            ScreenBounds = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
            
            ShipFactory shipFactory = new ShipFactory(_shipConfig, _bulletsConfig, _laserConfig, _gameLoop);
            shipFactory.CreateShip(new Vector3(ScreenBounds.x / 2, ScreenBounds.y / 2));
            ShipKeyboardController shipKeyboardController = new ShipKeyboardController(shipFactory.Ship, shipFactory.ShipFire);
            ShipUIController shipUI = new ShipUIController(shipFactory.Ship, shipFactory.ShipFire, _shipUI);
            AsteroidFactory asteroidsFactory = new AsteroidFactory(_asteroidsConfig, _smallAsteroidConfig,_gameLoop);
            asteroidsFactory.Initialize();
            UFOFactory ufoFactory = new UFOFactory(_ufoConfig, shipFactory.ShipTransform, _gameLoop);
            ufoFactory.Initialize();

            _gameLoop.AddToStarable(shipUI);
            _gameLoop.AddToStarable(shipFactory.Ship);
            _gameLoop.AddToStarable(shipFactory.ShipFire);
            _gameLoop.AddToUpdatable(shipKeyboardController);
            _gameLoop.AddToUpdatable(shipFactory.ShipFire);
            _gameLoop.AddToUpdatable(asteroidsFactory);
            _gameLoop.AddToUpdatable(ufoFactory);
            _gameLoop.AddToFixedUpdatable(shipFactory.Ship);

            _shipUI.AddListenerToRestartButton(ReloadGame);
        }

        private void Start()
        {
            _gameLoop.Start();
        }

        private void Update()
        {
            _gameLoop.Update();
        }

        private void FixedUpdate()
        {
            _gameLoop.FixedUpdate();
        }

        private void ReloadGame()
        {
            _sceneSecretary.ReloadSceneAsSingle();
        }
    }

    public class GameLoop
    {
        private List<IStartable> _startable = new List<IStartable>();
        private List<IUpdatable> _updatable = new List<IUpdatable>();
        private List<IFixedUpdatable> _fixedUpdatable = new List<IFixedUpdatable>();

        public void Start()
        {
            for (int i = 0; i < _startable.Count; i++)
                _startable[i].Start();
            
        }

        public void Update()
        {
            for (int i = 0; i < _updatable.Count; i++)
                _updatable[i].Update();
        }

        public void FixedUpdate()
        {
            for (int i = 0; i < _fixedUpdatable.Count; i++)
                _fixedUpdatable[i].FixedUpdate();
        }

        public void AddToStarable(IStartable startable)
        {
            _startable.Add(startable);
        }

        public void AddToUpdatable(IUpdatable updatable)
        {
            _updatable.Add(updatable);
        }

        public void RemoveFromUpdatable(IUpdatable updatable)
        {
            _updatable.Remove(updatable);
        }

        public void AddToFixedUpdatable(IFixedUpdatable fixedUpdatable)
        {
            _fixedUpdatable.Add(fixedUpdatable);
        }
    }

    public class SceneSecretary
    {
        private const int MainSceneIndex = 0;
        public void ReloadSceneAsSingle()
        {
            SceneManager.LoadScene(MainSceneIndex, LoadSceneMode.Single);
        }
    }

    public class ShipUIController : IStartable
    {
        private Ship _ship;
        private ShipFire _shipFire;
        private ShipUI _shipUI;

        public ShipUIController(Ship ship, ShipFire shipFire, ShipUI shipUI)
        {
            _ship = ship;
            _shipFire = shipFire;
            _shipUI = shipUI;
        }

        public void Start()
        {
            Bind();
        }

        private void Bind()
        {
            _ship.PositionChanged += _shipUI.OnPositionChanged;
            _ship.RotationChanged += _shipUI.OnRotationChanged;
            _ship.SpeedChanged += _shipUI.OnSpeedChanged;
            _ship.Die += _shipUI.OnGameOver;
            _shipFire.LaserCountChanged += _shipUI.OnLaserCountChanged;
            _shipFire.LaserReloadTimeChanged += _shipUI.OnLaserReloadTimeChanged;
        }

        private void Unbind()
        {
            _ship.PositionChanged -= _shipUI.OnPositionChanged;
            _ship.RotationChanged -= _shipUI.OnRotationChanged;
            _ship.SpeedChanged -= _shipUI.OnSpeedChanged;
            _ship.Die -= _shipUI.OnGameOver;
            _shipFire.LaserCountChanged -= _shipUI.OnLaserCountChanged;
            _shipFire.LaserReloadTimeChanged -= _shipUI.OnLaserReloadTimeChanged;
        }
    }
}
