using Assets.Scripts;
using System;
using UnityEngine;

namespace Assets.Models
{
    public class Laser: IUpdatable
    {
        public bool IsFiering = false;

        private LaserView _view;
        private GameLoop _gameLoop;
        private float _laserLifeTime;
        private float _ellapsedTime = 0f;

        public event Action<Laser> Died;

        public Laser(LaserView prefab, FireConfig fireConfig, float laserLifeTime, GameLoop gameLoop)
        {
            _view = LaserView.Instantiate(prefab, fireConfig.FirePosition, Quaternion.identity, fireConfig.Transform);
            _view.gameObject.SetActive(false);
            _view.Initialize(this);
            this._laserLifeTime = laserLifeTime;
            _gameLoop = gameLoop;
        }

        public void Update()
        {
            _ellapsedTime += Time.deltaTime;

            if(_ellapsedTime >= _laserLifeTime)
            {
                _ellapsedTime = 0;
                IsFiering = false;
                Died?.Invoke(this);
            }
        }

        internal void Fire()
        {
            IsFiering = true;
            _view.gameObject.SetActive(true);
            _gameLoop.AddToUpdatable(this);
            Died += _gameLoop.RemoveFromUpdatable;
        }
    }
}