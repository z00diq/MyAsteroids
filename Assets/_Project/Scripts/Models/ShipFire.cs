using System;
using Zenject;

namespace Assets.Models
{
    public class ShipFire : IInitializable, IDisposable
    {
        private readonly BulletSpawner _bulletSpawner;
        private readonly LaserFireController _laserSpawner;
        
        private int _laserFireCount;
        private float _laserEllapsedTime;

        public event Action FireBullet;
        public event Action FireLaser;

        public ShipFire(BulletSpawner bulletSpawner, LaserFireController laserSpawner)
        {
            _bulletSpawner = bulletSpawner;
            _laserSpawner = laserSpawner;     
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
            FireLaser += _laserSpawner.Fire;
            FireBullet += _bulletSpawner.Spawn;
        }

        void IDisposable.Dispose()
        {
            FireBullet -= _bulletSpawner.Spawn;
            FireLaser -= _laserSpawner.Fire;
        }
    }
}