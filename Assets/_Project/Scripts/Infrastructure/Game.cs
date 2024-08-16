using Assets.Infrastructure;
using Assets.Models;
using UnityEngine;

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
}
