using Assets.Scripts;
using Assets.Views;
using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Models
{
    public class ShipFire : IUpdatable, IStartable
    {
        private ObjectPool<Bullet> _bullets;
        private BulletsFireConfigure _bulletsConfiguration;
        private LaserFireConfigure _laserConfiguration;
        private GameLoop _gameLoop;
        private Laser _activeLaser;

        private FireConfig _fireConfig;
        
        private int _laserFireCount;
        private bool _readyToFire = true;
        private float _laserEllapsedTime;
        private float _bulletEllapsedTime;

        public event Action<int> LaserCountChanged;
        public event Action<float> LaserReloadTimeChanged;

        public ShipFire(FireConfig fireConfig, BulletsFireConfigure bulletsConfiguration, LaserFireConfigure laserConfiguration, GameLoop gameLoop)
        {
            _fireConfig = fireConfig;
            _laserConfiguration = laserConfiguration;
            _bulletsConfiguration = bulletsConfiguration;
            _laserFireCount = laserConfiguration.FireMaxCount;
            _bullets = new ObjectPool<Bullet>(CreateBullet, OnTakeFromPool, OnReturnedToPool);
            _gameLoop = gameLoop;
            _activeLaser = new Laser(_laserConfiguration.Prefab as LaserView, _fireConfig, _laserConfiguration.LifeTime, _gameLoop);
        }

        public void FireBullet()
        {
            if (_readyToFire == false)
                return;

            _bullets.Get();

            _readyToFire = false;
        }

        public void LaserFire()
        {
            if (_laserFireCount == 0 || _activeLaser.IsFiering)
                return;

            _activeLaser.Fire();
            _laserFireCount--;
            LaserCountChanged?.Invoke(_laserFireCount);
        }

        public void Start()
        {
            LaserCountChanged?.Invoke(_laserFireCount);
            LaserReloadTimeChanged?.Invoke(_laserEllapsedTime);
        }

        public void Update()
        {
            _bulletEllapsedTime += Time.deltaTime;

            if (_bulletEllapsedTime >= _bulletsConfiguration.ReloadTime)
            {
                _readyToFire = true;
                _bulletEllapsedTime = 0;
            }

            if (_laserFireCount < _laserConfiguration.FireMaxCount)
            {
                _laserEllapsedTime += Time.deltaTime;

                if (_laserEllapsedTime >= _laserConfiguration.ReloadTime)
                {
                    _laserFireCount++;
                    LaserCountChanged?.Invoke(_laserFireCount);
                    _laserEllapsedTime = 0;
                }

                LaserReloadTimeChanged?.Invoke(_laserEllapsedTime);
            }
        }

        private Bullet CreateBullet()
        {
            BulletView bulletView = UnityEngine.Object.Instantiate(_bulletsConfiguration.Prefab as BulletView, _fireConfig.FirePosition, _fireConfig.Transform.rotation);
            Bullet bullet = new Bullet(_bulletsConfiguration.Speed,bulletView.transform,bulletView.Size);
            bulletView.Initialize(bullet);
            bullet.OutFromBounds += _bullets.Release;
            return bullet;
        }

        private void OnTakeFromPool(Bullet bullet)
        {
            bullet.SetPosition(_fireConfig.FirePosition);
            bullet.SetRotation(_fireConfig.Transform.rotation);
            bullet.SetActive(true);
            _gameLoop.AddToUpdatable(bullet);
        }

        private void OnReturnedToPool(Bullet bullet)
        {
            _gameLoop.RemoveFromUpdatable(bullet);
            bullet.SetActive(false);
        }
    }
}