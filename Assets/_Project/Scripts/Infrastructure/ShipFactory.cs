using Assets.Configurations;
using Assets.Models;
using Assets.Views;
using UnityEngine;
using Zenject;

namespace Assets.Infrastructure
{
    public class ShipFactory : IFactory<Ship>, IFactory<ShipFire>
    {
        private readonly DiContainer _container;
        private readonly BulletsFireConfig _bulletsConfiguration;
        private readonly LaserFireConfig _laserConfiguration;

        public ShipFactory(DiContainer container, ShipConfig shipConfigure, BulletsFireConfig bulletsFireConfigure, LaserFireConfig laserFireConfigure)
        {
            _container = container;
            _bulletsConfiguration = bulletsFireConfigure;
            _laserConfiguration = laserFireConfigure;
        }

        Ship IFactory<Ship>.Create() 
        {
            ShipConfig shipConfig = _container.Resolve<ShipConfig>();
            ShipView shipView = _container.Resolve<ShipView>();
            Ship ship = new Ship(shipView.GetComponent<Rigidbody>(), shipView.Size, shipConfig);
            shipView.Initialize(ship);

            return ship;
        }

        ShipFire IFactory<ShipFire>.Create()
        {
            FireConfig config = _container.Resolve<FireConfig>();
            BulletSpawner bulletSpawner = _container.Resolve<BulletSpawner>();
            LaserFireController laserFire = _container.Resolve<LaserFireController>();
            ShipFire shipFire = new ShipFire(bulletSpawner, laserFire);

            return shipFire;
        }
    }

    public class ShipViewFactory : IFactory<ShipView>
    {
        private readonly ShipView _prefab;
        private readonly Vector3 _spawnPosition;

        public ShipViewFactory(
            ShipView prefab, 
            [Inject(Id ="Ship Spawn Position")]Vector3 spawnPosition)
        {
            _prefab = prefab;
            _spawnPosition = spawnPosition;
        }

        ShipView IFactory<ShipView>.Create()
        {
            return Object.Instantiate(_prefab, _spawnPosition, Quaternion.identity);
        }
    }
}