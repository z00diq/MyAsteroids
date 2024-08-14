using UnityEngine;

namespace Assets.Models
{
    internal class UFO : BaseEnemy
    {
        private Transform _target;
        public UFO(Transform target,float minSpeed, float maxSpeed, float tooFarDistance) : base(minSpeed, maxSpeed, tooFarDistance)
        {
            _target = target;
        }

        public override void SetPosition(Vector3 position)
        {
            base.SetPosition(position);
        }

        protected override void CalculateDeltaMove()
        {
            Position = Vector3.MoveTowards(Position, _target.position, Speed * Time.deltaTime);
        }

        protected override void CalculateMoveSettings()
        {
            Speed = Random.Range(MinSpeed, MaxSpeed);
        }

        protected override void TrySendCallback()
        {
            if (Extensions.IsPositionTooFar(Position, View, TooFarDistance))
                OutFromBounds?.Invoke(this);
        }
    }
}
