using Assets.Scripts;
using System;
using UnityEngine;
using Random=UnityEngine.Random;

namespace Assets.Models
{
    public class BaseEnemy : Destroyable, IUpdatable
    {
        public Action<BaseEnemy> OutFromBounds;

        protected Transform Transform;
        protected Vector2 ModelSize;
        protected Vector3 MoveVector;
        protected float Speed;
        protected float MinSpeed;
        protected float MaxSpeed;
        protected float TooFarDistance;

        public BaseEnemy(EnemyConfiguration configuration, EnemyView view)
        {
            Transform = view.transform;
            ModelSize = view.ModelSize;
            MinSpeed = configuration.MinSpeed;
            MaxSpeed = configuration.MaxSpeed;
            TooFarDistance = configuration.OutBoundsDepth;
        }

        public event Action<Vector3> Moved;
        public event Action Died;
        public event Action EnableGameObject;
        public event Action DisableGameObject;

        public void Update()
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

        public override void TakeDamage(GunShot gunShot)
        {
            Died?.Invoke();
        }

        public virtual void SetPosition(Vector3 position)
        {
            Moved?.Invoke(Transform.position);
        }

        public void TriggerDetected(Collider collider)
        {
            if (collider.TryGetComponent(out BaseView<GunShot> gunShotView))
                TakeDamage(gunShotView.RenderSize);
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
            if(Utilities.IsPositionTooFar(Transform.position, ModelSize, TooFarDistance))
                Died?.Invoke();
        }

        protected virtual void CalculateDeltaMove()
        {
            Vector3 deltaMoveVector = MoveVector * Time.deltaTime;
            Transform.position += deltaMoveVector;
        }

        private void Move()
        {
            CalculateDeltaMove();
            Moved?.Invoke(Transform.position);
            IsAsteroidTooFar();
        }
    }
}
