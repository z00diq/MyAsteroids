using Assets;
using Assets.Infrastructure;
using Assets.Models;
using Assets.Scripts;
using UnityEngine;

public class AsteroidFactory : Factory<Asteroid>
{
    public AsteroidFactory(AsteroidConfiguration configuration) : base(configuration)
    {
    }

    public override Asteroid CreateEnemy()
    {
        Vector3 spawnPosition = Utilities.CalculatePositionOutsideBounds(Configuration.OutBoundsDepth);

        AsteroidView asteroidView = Object.Instantiate(Configuration.Prefab, spawnPosition, Quaternion.identity, Game.Instance.gameObject.transform) as AsteroidView;
        Asteroid enemy = new Asteroid(Configuration as AsteroidConfiguration, asteroidView);
        asteroidView.Initialize(enemy);
        enemy.CalculateMoveSettings();
        enemy.OutFromBounds += OnOutFromBounds;
        enemy.Died += Enemies.Dispose;


        return enemy;
    }


}
