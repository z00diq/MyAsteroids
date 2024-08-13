using Assets.Models;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Infrastructure
{
    public class AsteroidsFactory : IUpdatable
    {
        private int _maxCount;
        private float _minSpeed;
        private float _maxSpeed;
        private AsteroidView _prefab;
        private float _occurrenceFrequency;
        private float _outBoundsDepth;

        private ObjectPool<Asteroid> _asteroids;
        private float _ellapsedTime = 0f;

        public AsteroidsFactory(int maxCount, float minSpeed,float maxSpeed, AsteroidView prefab, float occurrenceFrequency, float outBoundsDepth)
        {
            _maxCount = maxCount;
            _minSpeed = minSpeed;
            _maxSpeed = maxSpeed;
            _prefab = prefab;
            _occurrenceFrequency = occurrenceFrequency;
            _outBoundsDepth = outBoundsDepth;
        }

        public void Initialize()
        {
            _asteroids = new ObjectPool<Asteroid>(CreateAsteroid,OnTakeFromPool,OnReturnedToPool,
                OnDestroyAsteroid, true,_maxCount,_maxCount);
        }

        public void Update()
        {
            _ellapsedTime += Time.deltaTime;

            if (_ellapsedTime >= _occurrenceFrequency)
            {
                _asteroids.Get();
                _ellapsedTime = 0;
            }
        }

        private Asteroid CreateAsteroid()
        {
            Vector3 spawnPosition = Extensions.CalculatePositionOutsideBounds(_outBoundsDepth, _prefab);
            Asteroid asteroid = new Asteroid(_minSpeed, _maxSpeed, _outBoundsDepth * 1.5f);
            AsteroidView asteroidView = AsteroidView.Instantiate(_prefab, spawnPosition, Quaternion.identity, Game.Instance.gameObject.transform);
            asteroid.Initialize(asteroidView);
            asteroid.OutFromBounds += _asteroids.Release;
            asteroid.Died += _asteroids.Dispose;
            return asteroid;
        }

        private void OnTakeFromPool(Asteroid asteroid)
        {
            asteroid.ViewGameObject.SetActive(true);
            Game.Instance.AddToUpdatable(asteroid);
        }

        private void OnReturnedToPool(Asteroid asteroid)
        {
            
            Vector3 newPosition = Extensions.CalculatePositionOutsideBounds(_outBoundsDepth, _prefab);
            asteroid.SetPosition(newPosition);
            Game.Instance.RemoveFromUpdatable(asteroid);
            asteroid.ViewGameObject.SetActive(false);
        }

        private void OnDestroyAsteroid(Asteroid asteroid)
        {
            Game.Instance.RemoveFromUpdatable(asteroid);
            asteroid.OutFromBounds-=_asteroids.Release;
            asteroid.Died-=_asteroids.Dispose;
        }
    }
}
