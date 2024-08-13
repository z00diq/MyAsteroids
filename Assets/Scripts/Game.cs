using Assets.Infrastructure;
using Assets.Models;
using Assets.Views;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Game:MonoBehaviour
    {
        private static Game _instance = null;
        public static Game Instance => _instance;

        [Header("Asteroid Settings")]
        [SerializeField] private AsteroidView _asteroidPrefab;
        [SerializeField] private int _maxCount;
        [SerializeField] private float _minSpeed;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _occurrenceFrequency;
        [SerializeField] private float _outBoundsDepth;

        [Header("UFO Settings")]
        [SerializeField] private UFOView _ufoPrefab;
        [SerializeField] private int _maxUFOCount;
        [SerializeField] private float _minUFOSpeed;
        [SerializeField] private float _maxUFOSpeed;
        [SerializeField] private float _occurrenceUFOFrequency;
        [SerializeField] private float _outUFOBoundsDepth;

        [Header("Ship Settings")]
        [SerializeField] private ShipView _shipPrefab;
        [SerializeField] private float _maxShipSpeed;
        [SerializeField] private float _deltaSpeed;
        [SerializeField] private float _rotationSpeed;

        [Header("Ship Fire Settings")]
        [SerializeField] private BulletView _bulletPrefab;
        [SerializeField] private LaserView _laserPrefab;
        [SerializeField] private int _laserFireCount;
        [SerializeField] private float _laserReloadTime;
        [SerializeField] private float _laserLifeTime;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private float _bulletReloadTime;

        private List<IStartable> _startable = new List<IStartable>();
        private List<IUpdatable> _updatable = new List<IUpdatable>();
        private List<IFixedUpdatable> _fixedUpdatable = new List<IFixedUpdatable>();

        private InputSystem _inputSystem;

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
            ShipView shipView = Instantiate(_shipPrefab, new Vector3(ScreenBounds.x / 2, ScreenBounds.y / 2), Quaternion.identity, transform);
            ShipFire shipFire = new ShipFire(shipView, _bulletPrefab, _laserPrefab,_laserFireCount, _laserReloadTime, _laserLifeTime, _bulletSpeed, _bulletReloadTime);
            Ship ship = new Ship(_maxShipSpeed, _deltaSpeed, _rotationSpeed,_inputSystem,shipFire);
            ship.Initilize(shipView);

            _updatable.Add(shipFire);
            _fixedUpdatable.Add(ship);

            return shipView;
        }

        private void Awake()
        {
            _instance = FindObjectOfType<Game>();
            _inputSystem = new InputSystem();
            ScreenBounds = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
            ShipView ufoTarget = CreateShip();
            
            EnemyFactory<Asteroid,AsteroidView> asteroidsFactory = new EnemyFactory<Asteroid,AsteroidView>(null,_maxCount, _minSpeed, _maxSpeed,
                _asteroidPrefab, _occurrenceFrequency, _outBoundsDepth);
            asteroidsFactory.Initialize();
            _updatable.Add(asteroidsFactory);

            EnemyFactory<UFO, UFOView> ufoFactory = new EnemyFactory<UFO, UFOView>(ufoTarget.transform,_maxUFOCount,
                _minUFOSpeed,_maxUFOSpeed,_ufoPrefab,_occurrenceUFOFrequency,_outUFOBoundsDepth);
            ufoFactory.Initialize();
            _updatable.Add(ufoFactory);

            _updatable.Add(_inputSystem);
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
    }
}
