using Assets.Configurations;
using Assets.Infrastructure;
using Assets.Views;
using System;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Assets.Models
{
    public class LaserFireController : IInitializable, ITickable
    {
        private readonly DiContainer _container;
        private readonly LaserFireConfig _config;

        private Laser _laser;
        private float _ellapsedTime;
        private int _fireCount;

        public event Action<int> LaserCountChanged;
        public event Action<float> LaserReloadTimeChanged;

        public LaserFireController(DiContainer container, LaserFireConfig config)
        {
            _container = container;
            _config = config;
            _fireCount = _config.FireMaxCount;
        }

        public void Fire()
        {
            if (_fireCount == 0 || _laser.IsFiering)
                return;

            _laser.Fire();
            _fireCount--;
            LaserCountChanged?.Invoke(_fireCount);
            _container.Resolve<TickableManager>().Add(_laser);
        }

        void IInitializable.Initialize()
        {
            LaserCountChanged?.Invoke(_fireCount);
            Create();
        }

        void ITickable.Tick()
        {
            if (_fireCount < _config.FireMaxCount)
            {
                _ellapsedTime += Time.deltaTime;

                if (_ellapsedTime >= _config.ReloadTime)
                {
                    _fireCount++;
                    LaserCountChanged?.Invoke(_fireCount);
                    _ellapsedTime = 0;
                }

                LaserReloadTimeChanged?.Invoke(_ellapsedTime);
            }
        }

        private void Create()
        {
            FireConfig fireConfig = _container.Resolve<FireConfig>();
            LaserView prefab = _config.Prefab as LaserView;
            LaserView view = Object.Instantiate(prefab, fireConfig.FirePosition, Quaternion.identity, fireConfig.Transform);
            view.gameObject.SetActive(false);
            _laser = new Laser(_config.LifeTime);
            _laser.EndFireing += EndFire;
            view.Initialize(_laser);
        }

        private void EndFire()
        {
            _container.Resolve<TickableManager>().Remove(_laser);
        }
    }
}