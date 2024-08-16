using Assets.Models;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Infrastructure
{
    public abstract class Factory<T> : IUpdatable where T:BaseEnemy
    {
        private float _ellapsedTime = 0f;
        protected GameLoop GameLoop;
        protected readonly EnemyConfiguration Configuration;
        protected ObjectPool<T> Enemies;

        public Factory(EnemyConfiguration configuration, GameLoop gameLoop)
        {
            Configuration = configuration;
            GameLoop = gameLoop;
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
            GameLoop.AddToUpdatable(enemy);
        }

        protected void OnOutFromBounds(BaseEnemy enemy)
        {
            Enemies.Release(enemy as T);
        }

        protected void OnReturnedToPool(T enemy)
        {
            Vector3 newPosition = Utilities.CalculatePositionOutsideBounds(Configuration.OutBoundsDepth);
            enemy.SetPosition(newPosition);
            GameLoop.RemoveFromUpdatable(enemy);
            enemy.Disable();
        }

        protected void OnDestroyEnemy(T enemy)
        {
            GameLoop.RemoveFromUpdatable(enemy);
            enemy.OutFromBounds -= OnOutFromBounds;
            enemy.Died -= Enemies.Dispose;
        }
    }
}
