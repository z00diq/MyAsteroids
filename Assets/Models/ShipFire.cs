using Assets.Scripts;
using Assets.Views;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Models
{
    public class ShipFire : IUpdatable
    {
        private ObjectPool<Bullet> _bullets;
        private ShipView _view;
        private BulletView _bulletViewpPrefab;
        private LaserView _laserViewPrefab;
        private Laser _activeLaser;

        private int _laserFireCount;
        private float _laserReloadTime;
        private float _laserLifeTime;
        private float _bulletSpeed;
        private float _bulletRealoadTime;
        private float _laserEllapsedTime;
        private float _bulletEllapsedTime;
        private bool _readyToFire = true;

        public ShipFire(ShipView view, BulletView bulletViewpPrefab, LaserView laserViewprefab,int laserFireCount, float laserReloadTime, float laserLifeTime, float bulletSpeed, float bulletRealoadTime)
        {
            _view = view;
            _bulletViewpPrefab = bulletViewpPrefab;
            _laserFireCount = laserFireCount;
            _laserReloadTime = laserReloadTime;
            _laserLifeTime = laserLifeTime;
            _bulletSpeed = bulletSpeed;
            _bulletRealoadTime = bulletRealoadTime;
            _bullets = new ObjectPool<Bullet>(CreateBullet, OnTakeFromPool, OnReturnedToPool);
            _laserViewPrefab = laserViewprefab;
            _activeLaser = new Laser(_laserViewPrefab, _view, _laserLifeTime);
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
            if (_laserFireCount == 0 && _activeLaser.IsFiering)
                return;

            _activeLaser.Fire();
            _laserFireCount--;
        }

        public void Update()
        {
            _laserEllapsedTime += Time.deltaTime;
            _bulletEllapsedTime += Time.deltaTime;

            if (_laserEllapsedTime >= _laserReloadTime)
            {
                _laserFireCount++;
                _laserEllapsedTime = 0;
            }

            if (_bulletEllapsedTime >= _bulletRealoadTime)
            {
                _readyToFire = true;
                _bulletEllapsedTime = 0;
            }
        }

        private Bullet CreateBullet()
        {
            Bullet bullet = new Bullet(_bulletSpeed);
            BulletView bulletView = Object.Instantiate(_bulletViewpPrefab, _view.FirePosition, _view.transform.rotation, null);
            bullet.Initialize(bulletView);
            bullet.OutFromBounds += _bullets.Release;
            return bullet;
        }

        private void OnTakeFromPool(Bullet bullet)
        {

            bullet.SetPosition(_view.FirePosition);
            bullet.ViewGameObject.transform.rotation = _view.transform.rotation;
            bullet.ViewGameObject.SetActive(true);
            Game.Instance.AddToUpdatable(bullet);
        }

        private void OnReturnedToPool(Bullet bullet)
        {
            Game.Instance.RemoveFromUpdatable(bullet);
            bullet.ViewGameObject.SetActive(false);
        }
    }
}