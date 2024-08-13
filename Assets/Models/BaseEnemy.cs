using Assets.Scripts;
using System;
using UnityEngine;
using Random=UnityEngine.Random;

namespace Assets.Models
{
    public class BaseEnemy : Destroyable, IUpdatable
    {
        protected Vector3 Position;
        protected Vector3 MoveVector;
        protected float Speed;
        protected float MinSpeed;
        protected float MaxSpeed;
        protected float TooFarDistance;
        protected View View;
        
        public BaseEnemy(float minSpeed, float maxSpeed, float tooFarDistance) 
        {
            MinSpeed = minSpeed;
            MaxSpeed = maxSpeed;
            TooFarDistance = tooFarDistance;
        }

        public event Action<Vector3> Moved;
        public event Action Died;

        public GameObject ViewGameObject => View.gameObject;

        public void Initialize(View view)
        {
            view.Initialize();
            View = view;
            View.TriggerDetected += TriggerDetected;
            Position = view.transform.position;
            CalculateMoveSettings();
        }

        public void Update()
        {
            Move();
        }

        public virtual void SetPosition(Vector3 position)
        {
            Moved?.Invoke(Position);
        }

        public override void Destroy()
        {
            Died?.Invoke();
        }

        protected virtual void CalculateMoveSettings()
        {
            float angle = Random.Range(0, 361);

            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 direction = rotation * Vector3.up;
            Speed = Random.Range(MinSpeed, MaxSpeed);
            MoveVector = direction * Speed;
        }

        private void TriggerDetected(Collider collider)
        {
            if (collider.TryGetComponent(out IEnnemyInteractable interactable) == false)
                return;

            interactable.Impact(this);

        }

        protected virtual void Move()
        {
            Vector3 deltaMoveVector = MoveVector * Time.deltaTime;
            Position += deltaMoveVector;
            Moved?.Invoke(Position);

            if (Extensions.IsPositionTooFar(Position, View, TooFarDistance))
                Died?.Invoke();
        }
    }
}
