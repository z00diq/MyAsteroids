using Assets.Configurations;
using Assets.Models;
using Assets.Views;
using UnityEngine;
using Zenject;

namespace Assets.Infrastructure
{
    class UFOFactory : Factory<UFO>
    {
        public UFOFactory(DiContainer container, UFOConfig configuration) : base(container, configuration)
        {
        }

        protected override UFO CreateEnemy()
        {
            Vector3 spawnPosition = Utilities.CalculatePositionOutsideBounds(Configuration.OutBoundsDepth);

            EnemyView enemyView = Object.Instantiate(Configuration.Prefab, spawnPosition, Quaternion.identity);
            Transform target = Container.Resolve<ShipView>().transform;
            UFO enemy = new UFO(Configuration as UFOConfig, enemyView as UFOView , target);
            enemyView.Initialize(enemy);
            enemy.CalculateMoveSettings();
            enemy.OutFromBounds += OnOutFromBounds;
            enemy.Died += OnDestroyEnemy;
            return enemy;
        }
    }
}