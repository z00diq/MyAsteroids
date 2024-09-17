using Assets.Configurations;
using Assets.Models;
using Assets.Views;
using System.ComponentModel;
using UnityEngine;
using Zenject;

namespace Assets.Infrastructure
{
    public partial class AsteroidFactory : Factory<Asteroid>
    {
        private readonly ParticleAsteroidConfig _smallAsteroidConfig;
        public AsteroidFactory(TickableManager tickable, AsteroidConfig configuration, ParticleAsteroidConfig smallAsteroidConfig)
            : base(tickable, configuration)
        {
            _smallAsteroidConfig = smallAsteroidConfig;
        }

        protected override Asteroid CreateEnemy()
        {
            Vector3 spawnPosition = Utilities.CalculatePositionOutsideBounds(Configuration.OutBoundsDepth);

            AsteroidView asteroidView = Object.Instantiate(Configuration.Prefab, spawnPosition, Quaternion.identity) as AsteroidView;
            Asteroid enemy = new Asteroid(Configuration as AsteroidConfig, asteroidView);
            asteroidView.Initialize(enemy);
            enemy.CalculateMoveSettings();
            enemy.OutFromBounds += OnOutFromBounds;
            enemy.Died += OnDestroyEnemy;
            enemy.Splited += CreateParticles;

            return enemy;
        }

        private void CreateParticles(Asteroid asteroid)
        {
            for (int i = 0; i < _smallAsteroidConfig.SpawnCount; i++)
            {
                EnemyView enemyView = Object.Instantiate(_smallAsteroidConfig.Prefab, asteroid.Position, Quaternion.identity);
                enemyView.gameObject.transform.localScale *= 0.3f;
                BaseEnemy baseEnemy = new BaseEnemy(_smallAsteroidConfig, enemyView);
                baseEnemy.CalculateMoveSettings();
                enemyView.Initialize(baseEnemy);
                TickableManager.Add(baseEnemy);
                asteroid.Splited -= CreateParticles;
            }
        }
    }
}