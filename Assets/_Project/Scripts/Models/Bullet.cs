using Assets.Infrastructure;
using Assets.Views;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Models
{
    public class Bullet: ITickable
    {
        private readonly Transform _transform;
        private readonly Vector3 _size;
        private readonly float _bulletSpeed;

        public event Action<bool> Enabled;
        public event Action<Bullet> OutFromBounds;

        public class Factroy : PlaceholderFactory<float, Transform, Vector3, Bullet> { }

        public Bullet(float bulletSpeed, Transform transform, Vector3 size)
        {
            _bulletSpeed = bulletSpeed;
            _transform = transform;
            _size = size;
        }

        public void SetActive(bool active) 
        {
            Enabled?.Invoke(active);
        }

        public void SetPosition(Vector3 firePosition)
        {
            _transform.position = firePosition;
        }

        public void SetRotation(Quaternion newRotation)
        {
            _transform.rotation = newRotation;
        }

        private void Move()
        {
            Vector3 deltaMove = _transform.up;
            deltaMove *= _bulletSpeed * Time.deltaTime;
            _transform.position += deltaMove;
          
            if (Utilities.IsPositionTooFar(_transform.position, _size))
                OutFromBounds?.Invoke(this);
        }

        void ITickable.Tick()
        {
            Move();
        }
    }
}
