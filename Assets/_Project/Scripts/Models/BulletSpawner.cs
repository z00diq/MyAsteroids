using Assets.Configurations;
using Assets.Infrastructure;
using Assets.Views;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Assets.Models
{
    public class BulletSpawner: ITickable
    {
        private readonly ObjectPool<Bullet> _bullets;
        private readonly DiContainer _container;
        private readonly FireConfig _fireConfig;
        private readonly Bullet.Factroy _bulletFactory;
        private readonly BulletView _prefab;
        private readonly BulletsFireConfig _config;
        private readonly float _realoadTime;

        private float _ellapsedTime;

        public BulletSpawner(DiContainer container, FireConfig fireConfig)
        {
            _container = container;
            _config = container.Resolve<BulletsFireConfig>();
            _bullets = new ObjectPool<Bullet>(CreateBullet, OnTakeFromPool, OnReturnedToPool);
            _fireConfig = fireConfig;
            _bulletFactory = container.Resolve<Bullet.Factroy>();
            _prefab = _config.Prefab as BulletView;
            _realoadTime = _config.ReloadTime;
            _ellapsedTime = _realoadTime;

        }

        private Bullet CreateBullet()
        {
            BulletView bulletView = UnityEngine.Object.Instantiate(_prefab, _fireConfig.FirePosition, _fireConfig.Transform.rotation);
            Bullet bullet = _bulletFactory.Create(_config.Speed, bulletView.transform, bulletView.Size);
            bulletView.Initialize(bullet);
            bullet.OutFromBounds += _bullets.Release;
            return bullet;
        }

        private void OnTakeFromPool(Bullet bullet)
        {
            bullet.SetPosition(_fireConfig.FirePosition);
            bullet.SetRotation(_fireConfig.Transform.rotation);
            _container.Resolve<TickableManager>().Add(bullet);
            bullet.SetActive(true);
        }

        private void OnReturnedToPool(Bullet bullet)
        {
            _container.Resolve<TickableManager>().Remove(bullet);
            bullet.SetActive(false);
        }

        public void Spawn()
        {
            if (_ellapsedTime < _realoadTime)
                return;

            _bullets.Get();
            _ellapsedTime = 0;
        }

        void ITickable.Tick()
        {
            _ellapsedTime += Time.deltaTime;
        }
    }
}