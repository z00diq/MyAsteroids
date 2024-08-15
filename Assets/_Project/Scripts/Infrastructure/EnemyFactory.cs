using Assets.Models;
using Assets.Scripts;
using Assets.Views;
using System;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Infrastructure
{
    public abstract class Factory<T> : IUpdatable where T:BaseEnemy
    {
        protected readonly EnemyConfiguration Configuration;
        protected ObjectPool<T> Enemies;
        private float _ellapsedTime = 0f;

        public Factory(EnemyConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Initialize()
        {
            Enemies = new ObjectPool<T>(CreateEnemy, OnTakeFromPool, OnReturnedToPool,
                OnDestroyEnemy, true, Configuration.MaxCount, Configuration.MaxCount);
        }

        public void Update()
        {
            _ellapsedTime += Time.deltaTime;

            if (_ellapsedTime >= Configuration.OccurrenceFrequency)
            {
                Enemies.Get();
                _ellapsedTime = 0;
            }
        }

        public abstract T CreateEnemy();
    
        protected void OnTakeFromPool(T enemy)
        {
            enemy.Enable();
            Game.Instance.AddToUpdatable(enemy);
        }

        protected void OnOutFromBounds(BaseEnemy enemy)
        {
            Enemies.Release(enemy as T);
        }

        protected void OnReturnedToPool(T enemy)
        {
            Vector3 newPosition = Utilities.CalculatePositionOutsideBounds(Configuration.OutBoundsDepth);
            enemy.SetPosition(newPosition);
            Game.Instance.RemoveFromUpdatable(enemy);
            enemy.Disable();
        }

        protected void OnDestroyEnemy(T enemy)
        {
            Game.Instance.RemoveFromUpdatable(enemy);
            enemy.OutFromBounds -= OnOutFromBounds;
            enemy.Died -= Enemies.Dispose;
        }
    }
}
