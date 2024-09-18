using System;
using UnityEngine;
using Zenject;

namespace Assets.Models
{
    public class Laser: ITickable
    {
        public bool IsFiering = false;

        private readonly float _laserLifeTime;
        private float _ellapsedTime = 0f;

        public event Action StartFiering;
        public event Action EndFireing;

        public Laser(float laserLifeTime)
        {
            _laserLifeTime = laserLifeTime;
        }

        public void Fire()
        {
            IsFiering = true;
            StartFiering?.Invoke();
        }

        void ITickable.Tick()
        {
            _ellapsedTime += Time.deltaTime;

            if (_ellapsedTime >= _laserLifeTime)
            {
                _ellapsedTime = 0;
                IsFiering = false;
                EndFireing?.Invoke();
            }
        }
    }
}