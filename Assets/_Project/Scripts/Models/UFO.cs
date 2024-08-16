using Assets.Scripts;
using Assets.Views;
using UnityEngine;

namespace Assets.Models
{
    internal class UFO : BaseEnemy
    {
        private Transform _target;

        public UFO(UFOConfiguration configuration, UFOView view, Transform target, GameLoop gameLoop) : base(configuration, view, gameLoop)
        {
            _target = target;
        }

        public override void SetPosition(Vector3 position)
        {
            base.SetPosition(position);
        }

        protected override void CalculateDeltaMove()
        {
            Transform.position = Vector3.MoveTowards(Transform.position, _target.position, Speed * Time.deltaTime);
        }

        public override void CalculateMoveSettings()
        {
            Speed = Random.Range(MinSpeed, MaxSpeed);
        }

        protected override void IsAsteroidTooFar()
        {
            if (Utilities.IsPositionTooFar(Transform.position, ModelSize, TooFarDistance))
                OutFromBounds?.Invoke(this);
        }
    }
}
