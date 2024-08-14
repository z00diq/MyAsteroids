using Assets.Scripts;
using Assets.Views;
using System;
using UnityEngine;

namespace Assets.Models
{
    public class Laser: IUpdatable
    {
        public bool IsFiering = false;

        private LaserView _view;
        private float _laserLifeTime;
        private float _ellapsedTime = 0f;

        public event Action<Laser> Died;

        public Laser(LaserView prefab, ShipView shipView,float laserLifeTime)
        {
            _view = LaserView.Instantiate(prefab, shipView.FirePosition, Quaternion.identity, shipView.transform);
            _view.gameObject.SetActive(false);
            _view.Initialize(this);
            this._laserLifeTime = laserLifeTime;
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
            Game.Instance.AddToUpdatable(this);
            Died += Game.Instance.RemoveFromUpdatable;
        }
    }
}