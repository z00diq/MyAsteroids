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

        [Header("UFO Settings")]
        [SerializeField] private UFOConfiguration _ufoConfig;

        [Header("Ship Settings")]
        [SerializeField] private ShipConfigure _shipConfig;


        [Header("Ship Fire Settings")]
        [SerializeField] private BulletView _bulletPrefab;
        [SerializeField] private LaserView _laserPrefab;
        [SerializeField] private int _laserFireCount;
        [SerializeField] private float _laserReloadTime;
        [SerializeField] private float _laserLifeTime;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private float _bulletReloadTime;

        [SerializeField] private CanvasEvents _canvasEvents;

        private List<IStartable> _startable = new List<IStartable>();
        private List<IUpdatable> _updatable = new List<IUpdatable>();
        private List<IFixedUpdatable> _fixedUpdatable = new List<IFixedUpdatable>();

        private ShipKeyboardController _inputSystem;
        private Ship _ship;
        private ShipFire _shipFire;
        private ShipView _shipView;

        public Vector2 ScreenBounds { get; private set; }

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

        private ShipView CreateShip()
        {
            _shipView = Instantiate(_shipConfig.Prefab, new Vector3(ScreenBounds.x / 2, ScreenBounds.y / 2), Quaternion.identity, transform);
            _shipFire = new ShipFire(_shipView, _bulletPrefab, _laserPrefab,_laserFireCount, _laserReloadTime, _laserLifeTime, _bulletSpeed, _bulletReloadTime);
            _ship = new Ship(_shipView.GetComponent<Rigidbody>(), _shipView.ModelSize, _shipConfig);
            _shipView.Initialize(_ship);
            _startable.Add(_ship);
            _startable.Add(_shipFire);
            _updatable.Add(_shipFire);
            _fixedUpdatable.Add(_ship);

            _ship.PositionChanged += _canvasEvents.OnPositionChanged;
            _ship.RotationChanged += _canvasEvents.OnRotationChanged;
            _ship.SpeedChanged += _canvasEvents.OnSpeedChanged;
            _ship.Die +=  _canvasEvents.OnGameOver;
            _shipFire.LaserCountChanged += _canvasEvents.OnLaserCountChanged;
            _shipFire.LaserReloadTimeChanged += _canvasEvents.OnLaserReloadTimeChanged;

            return _shipView;
        }

        private void Awake()
        {
            _instance = FindObjectOfType<Game>();
            ScreenBounds = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
            ShipView ufoTarget = CreateShip();
            _inputSystem = new ShipKeyboardController(_ship,_shipFire);
            
            AsteroidFactory asteroidsFactory = new AsteroidFactory(_asteroidsConfig);
            asteroidsFactory.Initialize();
            _updatable.Add(asteroidsFactory);

            UFOFactory ufoFactory = new UFOFactory(_ufoConfig, _shipView.transform);
            ufoFactory.Initialize();
            _updatable.Add(ufoFactory);

            _updatable.Add(_inputSystem);

            _canvasEvents.AddListenerToRestartButton(ReloadGame);
        }

        private void Start()
        {
            for (int i = 0; i < _startable.Count; i++)
            {
                _startable[i].Start();
            }
        }

        private void Update()
        {
            for (int i = 0; i < _updatable.Count; i++)
            {
                _updatable[i].Update();
            }
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < _fixedUpdatable.Count; i++)
            {
                _fixedUpdatable[i].FixedUpdate();
            }
        }

        private void ReloadGame()
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }
}
