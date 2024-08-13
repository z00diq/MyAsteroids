using Assets.Scripts;
using System;
using UnityEngine;
using Random=UnityEngine.Random;

namespace Assets.Models
{
    public class BaseEnemy : Destroyable, IUpdatable
    {
        public Action<BaseEnemy> OutFromBounds;
        
        protected Vector3 Position;
        protected Vector3 MoveVector;
        protected float Speed;
        protected float MinSpeed;
        protected float MaxSpeed;
        protected float TooFarDistance;
        protected EnemyView View;

        public BaseEnemy(float minSpeed, float maxSpeed, float tooFarDistance) 
        {
            MinSpeed = minSpeed;
            MaxSpeed = maxSpeed;
            TooFarDistance = tooFarDistance;
        }

        public event Action<Vector3> Moved;
        public event Action Died;
        public event Action EnableGameObject;
        public event Action DisableGameObject;

        public GameObject ViewGameObject => View.gameObject;

        public void Initialize(EnemyView view)
        {
            view.Initialize(this);
            View = view;
            View.TriggerDetected += TriggerDetected;
            Position = view.transform.position;
            CalculateMoveSettings();
        }

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

        public override void Destroy()
        {
            Died?.Invoke();
        }

        public virtual void SetPosition(Vector3 position)
        {
            Moved?.Invoke(Position);
        }

        protected virtual void CalculateMoveSettings()
        {
            float angle = Random.Range(0, 361);

            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 direction = rotation * Vector3.up;
            Speed = Random.Range(MinSpeed, MaxSpeed);
            MoveVector = direction * Speed;
        }

        protected virtual void TrySendCallback()
        {
            if(Extensions.IsPositionTooFar(Position, View, TooFarDistance))
                Died?.Invoke();
        }

        protected virtual void CalculateDeltaMove()
        {
            Vector3 deltaMoveVector = MoveVector * Time.deltaTime;
            Position += deltaMoveVector;
        }

        private void TriggerDetected(Collider collider)
        {
            if (collider.TryGetComponent(out IEnnemyInteractable interactable) == false)
                return;

            interactable.Impact(this);

        }

        private void Move()
        {
            CalculateDeltaMove();
            Moved?.Invoke(Position);
            TrySendCallback();
        }
    }
}
