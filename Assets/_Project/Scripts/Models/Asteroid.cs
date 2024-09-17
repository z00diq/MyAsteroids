using Assets.Configurations;
using Assets.Views;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Models
{
    public class Asteroid: BaseEnemy
    {
        public event Action<Asteroid> Splited;
        public override event Action<BaseEnemy> OutFromBounds;

        public Asteroid(AsteroidConfig asteroidConfiguration, AsteroidView view) : 
            base(asteroidConfiguration, view)
        {
        }

        public override void SetPosition(Vector3 position)
        {
            CalculateMoveSettings();
            base.SetPosition(position);
        }

        public override void TakeDamage(DamageType damageType)
        {
            if(damageType==DamageType.Bullet)
                Splited?.Invoke(this);

            base.TakeDamage(damageType);
        }

        public override void CalculateMoveSettings()
        {
            Vector3 target = new Vector3(Utilities.ScreenBounds.x / 2, Utilities.ScreenBounds.y / 2);
            Vector3 direction = (target - Position).normalized;
            Speed = Random.Range(MinSpeed, MaxSpeed);
            MoveVector = direction * Speed;
        }

        protected override void IsAsteroidTooFar()
        {
            if (Utilities.IsPositionTooFar(Position, ModelSize, TooFarDistance))
                OutFromBounds?.Invoke(this);
        }
    }
}
