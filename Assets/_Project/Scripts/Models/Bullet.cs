using Assets.Scripts;
using System;
using UnityEngine;

namespace Assets.Models
{
    public class Bullet: IUpdatable
    {
        private float _bulletSpeed;
        private Transform _transform;
        private Vector3 _size;

        public event Action<bool> Enabled;
        public event Action<Bullet> OutFromBounds;

        public Bullet(float bulletSpeed, Transform transform, Vector3 size)
        {
            _bulletSpeed = bulletSpeed;
            _transform = transform;
            _size = size;
        }

        public void Update()
        {
            Move();
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
    }
}
