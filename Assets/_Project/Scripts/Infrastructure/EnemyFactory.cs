using Assets._Project.Scripts.Ads;
using Assets._Project.Scripts.Remotes;
using Assets.Configurations;
using Assets.Models;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;
using static UnityEngine.EventSystems.EventTrigger;

namespace Assets.Infrastructure
{
    public abstract class Factory<T> : IInitializable,ITickable,IDisposable where T:BaseEnemy
    {
        protected readonly TickableManager TickableManager;
        protected readonly EnemyConfig Configuration;
        protected readonly RemoteAnalytics _analytics;
        protected ObjectPool<T> Enemies;
        private float _ellapsedTime = 0f;
        private Ads _ads;
        private List<T> _activeEnemies;
        protected Factory(TickableManager tickableManager, EnemyConfig configuration, RemoteAnalytics analytics,Ads ads)
        {
            TickableManager = tickableManager;
            Configuration = configuration;
            _analytics = analytics;
            _ads = ads;
        }

        void IInitializable.Initialize()
        {
            Enemies = new ObjectPool<T>(CreateEnemy, OnTakeFromPool, OnReturnedToPool,
                OnDestroyEnemy, true, Configuration.MaxCount, Configuration.MaxCount);
            _ads.SendReward += DestroyAllEnemy;
            _activeEnemies = new List<T>();
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
            _activeEnemies.Add(enemy);
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

            if ((enemy is UFO) == true)
                _analytics.IncreaseUfoDestroyedCount();
            else
                _analytics.IncreaseAsteroidsDestroyedCount();
        }

        private void OnReturnedToPool(T enemy)
        {
            TickableManager.Remove(enemy);
            Vector3 newPosition = Utilities.CalculatePositionOutsideBounds(Configuration.OutBoundsDepth);
            _activeEnemies.Remove(enemy);
            enemy.SetPosition(newPosition);
            enemy.Disable();
        }

        void IDisposable.Dispose()
        {
            _ads.SendReward -= DestroyAllEnemy;
        }

        private void DestroyAllEnemy()
        {
            for (int i = 0; i < _activeEnemies.Count; i++)
            {
                OnReturnedToPool(_activeEnemies[i--]);
            }
        }
    }
}
