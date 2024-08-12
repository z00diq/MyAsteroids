using Assets.Infrastructure;
using Assets.Models;
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
        [SerializeField] private AsteroidView _prefab;
        [SerializeField] private int _maxCount;
        [SerializeField] private float _minSpeed;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _occurrenceFrequency;
        [SerializeField] private float _outBoundsDepth;
        
        private List<IStartable> _startable = new List<IStartable>();
        private List<IUpdatable> _updatable = new List<IUpdatable>();

        public Vector2 ScreenBounds { get; private set; }


        public void AddToStarable(IStartable startable)
        {
            _startable.Add(startable);
        }

        public void AddToUpdatable(IUpdatable updatable)
        {
            _updatable.Add(updatable);
        }

        public void RemoveFromUpdatable(Asteroid asteroid)
        {
            _updatable.Remove(asteroid);
        }

        private void Awake()
        {
            _instance = FindObjectOfType<Game>();
            ScreenBounds = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
            AsteroidsFactory asteroidsFactory = new AsteroidsFactory(_maxCount, _minSpeed, _maxSpeed,
                _prefab, _occurrenceFrequency, _outBoundsDepth);
            asteroidsFactory.Initialize();

            _updatable.Add(asteroidsFactory);
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
    }
}
