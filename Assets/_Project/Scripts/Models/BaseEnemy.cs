using Assets.Configurations;
using Assets.Views;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using Random=UnityEngine.Random;

namespace Assets.Models
{
    public class BaseEnemy : Destroyable, ITickable
    {
        public Action<BaseEnemy> OutFromBounds;

        protected readonly Vector2 ModelSize;
        protected readonly float MinSpeed;
        protected readonly float MaxSpeed;
        protected readonly float TooFarDistance;
        protected Vector3 MoveVector;
        protected float Speed;

        public Vector3 Position { get; protected set; }

        public BaseEnemy(EnemyConfig configuration, EnemyView view)
        {
            Position = view.transform.position;
            ModelSize = view.Size;
            MinSpeed = configuration.MinSpeed;
            MaxSpeed = configuration.MaxSpeed;
            TooFarDistance = configuration.OutBoundsDepth;

            Died += OnDie;
        }

        public event Action<Vector3> Moved;
        public event Action<BaseEnemy> Died;
        public event Action EnableGameObject;
        public event Action DisableGameObject;

        void ITickable.Tick()
        {
            Move();
        }

        public void Enable()
        {
            EnableGameObject?.Invoke();
        }

        public void Disable()
        {
            DisableGameObject?.Invoke();
        }

    

        public override void TakeDamage(DamageType damageType)
        {
            Died?.Invoke(this);
        }

        public virtual void SetPosition(Vector3 position)
        {
            Moved?.Invoke(position);
        }

        public void TriggerDetected(DamageType damageType)
        {
            TakeDamage(damageType);
        }

        public virtual void CalculateMoveSettings()
        {
            float angle = Random.Range(0, 361);

            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 direction = rotation * Vector3.up;
            Speed = Random.Range(MinSpeed, MaxSpeed);
            MoveVector = direction * Speed;
        }

        protected virtual void IsAsteroidTooFar()
        {
            if(Utilities.IsPositionTooFar(Position, ModelSize, TooFarDistance))
                Died?.Invoke(this);
        }

        protected virtual void CalculateDeltaMove()
        {
            Vector3 deltaMoveVector = MoveVector * Time.deltaTime;
            Position += deltaMoveVector;
        }

        private void Move()
        {
            CalculateDeltaMove();
            Moved?.Invoke(Position);
            IsAsteroidTooFar();
        }

        public void OnDie(BaseEnemy baseEnemy)
        {
            Died -= OnDie;
        }
    }
}
