using Assets._Project.Scripts.Ads;
using Assets._Project.Scripts.Remotes;
using Assets.Configurations;
using Assets.Models;
using Assets.Views;
using UnityEngine;
using Zenject;

namespace Assets.Infrastructure
{
    class UFOFactory : Factory<UFO>
    {
        private ShipView _target;

        public UFOFactory(TickableManager tickable, ShipView shipView,UFOConfig configuration, RemoteAnalytics remoteAnalytics, Ads ads) : 
            base(tickable, configuration, remoteAnalytics,ads)
        {
            _target = shipView;
        }

        protected override UFO CreateEnemy()
        {
            Vector3 spawnPosition = Utilities.CalculatePositionOutsideBounds(Configuration.OutBoundsDepth);

            EnemyView enemyView = Object.Instantiate(Configuration.Prefab, spawnPosition, Quaternion.identity);
            Transform target = _target.transform;
            UFO enemy = new UFO(Configuration as UFOConfig, enemyView as UFOView , target);
            enemyView.Initialize(enemy);
            enemy.CalculateMoveSettings();
            enemy.OutFromBounds += OnOutFromBounds;
            enemy.Died += OnDestroyEnemy;
            return enemy;
        }
    }
}