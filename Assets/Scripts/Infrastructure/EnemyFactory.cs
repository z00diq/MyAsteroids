using Assets.Models;
using Assets.Scripts;
using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Infrastructure
{
    public class EnemyFactory<T,V> : IUpdatable where V : EnemyView where T : BaseEnemy
    {
        private Transform _target;
        private int _maxCount;
        private float _minSpeed;
        private float _maxSpeed;
        private V _prefab;
        private float _occurrenceFrequency;
        private float _outBoundsDepth;

        private ObjectPool<T> _enemies;
        private float _ellapsedTime = 0f;

        public EnemyFactory(Transform target,int maxCount, float minSpeed,float maxSpeed, V prefab, float occurrenceFrequency, float outBoundsDepth)
        {
            _maxCount = maxCount;
            _minSpeed = minSpeed;
            _maxSpeed = maxSpeed;
            _prefab = prefab;
            _occurrenceFrequency = occurrenceFrequency;
            _outBoundsDepth = outBoundsDepth;
            _target = target;
        }

        public void Initialize()
        {
            _enemies = new ObjectPool<T>(CreateEnemy,OnTakeFromPool,OnReturnedToPool,
                OnDestroyEnemy, true,_maxCount,_maxCount);
        }

        public void Update()
        {
            _ellapsedTime += Time.deltaTime;

            if (_ellapsedTime >= _occurrenceFrequency)
            {
                _enemies.Get();
                _ellapsedTime = 0;
            }
        }

        private T CreateEnemy()
        {
            T enemy = null;
            Vector3 spawnPosition = Extensions.CalculatePositionOutsideBounds(_outBoundsDepth, _prefab);
 
            if (typeof(T) == typeof(Asteroid))
                enemy = (new Asteroid(_minSpeed, _maxSpeed, _outBoundsDepth * 1.5f) as T);
            else if(typeof(T)==typeof(UFO))
                enemy = (new UFO(_target, _minSpeed,_maxSpeed,_outBoundsDepth*1.5f) as T);
            
            V enemyView = GameObject.Instantiate<V>(_prefab, spawnPosition, Quaternion.identity, Game.Instance.gameObject.transform);
            enemy.Initialize(enemyView);
            enemy.OutFromBounds += OnOutFromBounds;
            enemy.Died += _enemies.Dispose;
            return enemy;
        }

        private void OnTakeFromPool(T enemy)
        {
            enemy.Enable();
            Game.Instance.AddToUpdatable(enemy);
        }

        private void OnOutFromBounds(BaseEnemy enemy)
        {
            _enemies.Release(enemy as T);
        }

        private void OnReturnedToPool(T enemy)
        {
            Vector3 newPosition = Extensions.CalculatePositionOutsideBounds(_outBoundsDepth, _prefab);
            enemy.SetPosition(newPosition);
            Game.Instance.RemoveFromUpdatable(enemy);
            enemy.Disable();
        }

        private void OnDestroyEnemy(T enemy)
        {
            Game.Instance.RemoveFromUpdatable(enemy);
            enemy.OutFromBounds-= OnOutFromBounds;
            enemy.Died-=_enemies.Dispose;
        }
    }
}
