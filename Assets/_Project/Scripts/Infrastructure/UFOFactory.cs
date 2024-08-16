using Assets;
using Assets.Infrastructure;
using Assets.Models;
using Assets.Scripts;
using Assets.Views;
using UnityEngine;

namespace Assets.Infrastructure
{
    class UFOFactory : Factory<UFO>
    {
        private Transform _target;

        public UFOFactory(UFOConfiguration configuration, Transform target, GameLoop gameLoop) : base(configuration, gameLoop)
        {
            _target = target;
        }

        public override UFO CreateEnemy()
        {
            Vector3 spawnPosition = Utilities.CalculatePositionOutsideBounds(Configuration.OutBoundsDepth);

            EnemyView enemyView = Object.Instantiate(Configuration.Prefab, spawnPosition, Quaternion.identity, Game.Instance.gameObject.transform);
            UFO enemy = new UFO(Configuration as UFOConfiguration, enemyView as UFOView , _target, GameLoop);
            enemyView.Initialize(enemy);
            enemy.CalculateMoveSettings();
            enemy.OutFromBounds += OnOutFromBounds;
            enemy.Died += Enemies.Dispose;
            return enemy;
        }
    }
}