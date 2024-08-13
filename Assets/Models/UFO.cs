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
            Vector3 deltaMoveVector = Vector3.MoveTowards(Position, _target.position, Speed * Time.deltaTime);
            Position += deltaMoveVector;
        }

        protected override void CalculateMoveSettings()
        {
            Speed = Random.Range(MinSpeed, MaxSpeed);
        }

        protected override void TrySendCallback()
        {
            base.TrySendCallback();
        }
    }
}
