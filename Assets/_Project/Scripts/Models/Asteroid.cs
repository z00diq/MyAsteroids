using Assets.Scripts;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Models
{
    public class Asteroid: BaseEnemy
    {
        public event Action<Asteroid> Die;

        public Asteroid(AsteroidConfiguration asteroidConfiguration, AsteroidView view) : 
            base(asteroidConfiguration, view)
        {
        }

        public override void SetPosition(Vector3 position)
        {
            CalculateMoveSettings();
            base.SetPosition(position);
        }

        public override void TakeDamage(GunShot gunShot)
        {
            if(gunShot is Bullet)
                Die?.Invoke(this);

            base.TakeDamage(gunShot);
        }

        public override void CalculateMoveSettings()
        {
            Vector3 target = new Vector3(Game.Instance.ScreenBounds.x / 2, Game.Instance.ScreenBounds.y / 2);
            Vector3 direction = (target - Transform.position).normalized;
            Speed = Random.Range(MinSpeed, MaxSpeed);
            MoveVector = direction * Speed;
        }

        protected override void IsAsteroidTooFar()
        {
            if (Utilities.IsPositionTooFar(Transform.position, ModelSize, TooFarDistance))
                OutFromBounds?.Invoke(this);
        }
    }
}
