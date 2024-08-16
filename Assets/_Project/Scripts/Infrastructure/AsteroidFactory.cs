using Assets;
using Assets.Infrastructure;
using Assets.Models;
using Assets.Scripts;
using UnityEngine;

public partial class AsteroidFactory : Factory<Asteroid>
{
    private readonly ParticleAsteroidConfiguration _smallAsteroidConfig;
    public AsteroidFactory(AsteroidConfiguration configuration, ParticleAsteroidConfiguration smallAsteroidConfig, GameLoop gameLoop) 
        : base(configuration, gameLoop)
    {
        _smallAsteroidConfig = smallAsteroidConfig;
    }

    public override Asteroid CreateEnemy()
    {
        Vector3 spawnPosition = Utilities.CalculatePositionOutsideBounds(Configuration.OutBoundsDepth);

        AsteroidView asteroidView = Object.Instantiate(Configuration.Prefab, spawnPosition, Quaternion.identity, Game.Instance.gameObject.transform) as AsteroidView;
        Asteroid enemy = new Asteroid(Configuration as AsteroidConfiguration, asteroidView, GameLoop);
        asteroidView.Initialize(enemy);
        enemy.CalculateMoveSettings();
        enemy.OutFromBounds += OnOutFromBounds;
        enemy.Died += Enemies.Dispose;
        enemy.Splited += CreateParticles;

        return enemy;
    }

    private void CreateParticles(Asteroid asteroid)
    {
        for (int i = 0; i < _smallAsteroidConfig.SpawnCount; i++)
        {
            EnemyView enemyView = Object.Instantiate(_smallAsteroidConfig.Prefab, asteroid.Position, Quaternion.identity);
            enemyView.gameObject.transform.localScale *= 0.3f;
            BaseEnemy baseEnemy = new BaseEnemy(_smallAsteroidConfig, enemyView,GameLoop);
            baseEnemy.CalculateMoveSettings();
            enemyView.Initialize(baseEnemy);
            GameLoop.AddToUpdatable(baseEnemy);
            asteroid.Splited -= CreateParticles;
        }
    }
}
