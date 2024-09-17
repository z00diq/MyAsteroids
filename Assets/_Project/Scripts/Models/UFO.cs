using Assets.Configurations;
using Assets.Views;
using UnityEngine;

namespace Assets.Models
{
    public class UFO : BaseEnemy
    {
        public override event System.Action<BaseEnemy> OutFromBounds;
        private readonly Transform _target;

        public UFO(UFOConfig configuration, UFOView view, Transform target) : base(configuration, view)
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

        public override void CalculateMoveSettings()
        {
            Speed = Random.Range(MinSpeed, MaxSpeed);
        }

        protected override void IsAsteroidTooFar()
        {
            if (Utilities.IsPositionTooFar(Position, ModelSize, TooFarDistance))
                OutFromBounds?.Invoke(this);
        }
    }
}
