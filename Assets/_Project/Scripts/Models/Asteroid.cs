using Assets.Scripts;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Models
{
    public class Asteroid: BaseEnemy
    {
        public event Action<Asteroid> Splited;

        public Asteroid(AsteroidConfiguration asteroidConfiguration, AsteroidView view, GameLoop gameLoop) : 
            base(asteroidConfiguration, view, gameLoop)
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
