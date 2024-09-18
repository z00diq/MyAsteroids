using Assets._Project.Scripts.Remotes;
using System;
using Zenject;

namespace Assets.Models
{
    public class ShipFire : IInitializable, IDisposable
    {
        private readonly BulletSpawner _bulletSpawner;
        private readonly LaserFireController _laserSpawner;
        private readonly RemoteAnalytics _remoteAnalytics;
        private int _laserFireCount;
        private float _laserEllapsedTime;

        public event Action FireBullet;
        public event Action FireLaser;

        public ShipFire(BulletSpawner bulletSpawner, LaserFireController laserSpawner, RemoteAnalytics remoteAnalytics)
        {
            _bulletSpawner = bulletSpawner;
            _laserSpawner = laserSpawner;
            _remoteAnalytics = remoteAnalytics;
        }

        public void BulletFire()
        {
            FireBullet?.Invoke();
        }

        public void LaserFire()
        {
            FireLaser?.Invoke();
        }

        void IInitializable.Initialize()
        {
            FireLaser += _remoteAnalytics.IncreaseLaserFireCount;
            FireBullet += _remoteAnalytics.IncreaseFireCount;
            FireLaser += _laserSpawner.Fire;
            FireBullet += _bulletSpawner.Spawn;
        }

        void IDisposable.Dispose()
        {
            FireLaser -= _remoteAnalytics.IncreaseLaserFireCount;
            FireBullet -= _remoteAnalytics.IncreaseFireCount;
            FireBullet -= _bulletSpawner.Spawn;
            FireLaser -= _laserSpawner.Fire;
        }
    }
}