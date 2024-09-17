using Assets.Configurations;
using Assets.Models;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Assets.Infrastructure
{
    public abstract class Factory<T> : IInitializable,ITickable where T:BaseEnemy
    {
        protected readonly TickableManager TickableManager;
        protected readonly EnemyConfig Configuration;
        protected ObjectPool<T> Enemies;
        private float _ellapsedTime = 0f;

        protected Factory(TickableManager tickableManager, EnemyConfig configuration)
        {
            TickableManager = tickableManager;
            Configuration = configuration;
        }

        void IInitializable.Initialize()
        {
            Enemies = new ObjectPool<T>(CreateEnemy, OnTakeFromPool, OnReturnedToPool,
                OnDestroyEnemy, true, Configuration.MaxCount, Configuration.MaxCount);
        }

        void ITickable.Tick()
        {
            _ellapsedTime += Time.deltaTime;

            if (_ellapsedTime >= Configuration.OccurrenceFrequency)
            {
                Enemies.Get();
                _ellapsedTime = 0;
            }
        }

        protected abstract T CreateEnemy();
    
        private void OnTakeFromPool(T enemy)
        {
            enemy.Enable();
            TickableManager.Add(enemy);
        }

        protected void OnOutFromBounds(BaseEnemy enemy)
        {
            Enemies.Release(enemy as T);
        }

        protected void OnDestroyEnemy(BaseEnemy enemy)
        {
            TickableManager.Remove(enemy);
            enemy.OutFromBounds -= OnOutFromBounds;
            enemy.Died -= OnDestroyEnemy;
        }

        private void OnReturnedToPool(T enemy)
        {
            TickableManager.Remove(enemy);
            Vector3 newPosition = Utilities.CalculatePositionOutsideBounds(Configuration.OutBoundsDepth);
            enemy.SetPosition(newPosition);
            enemy.Disable();
        }
    }
}
