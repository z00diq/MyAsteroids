using Assets.Configurations;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Models
{
    public class Ship : IInitializable, IFixedTickable
    {
        private readonly Rigidbody _rigidBody;
        private readonly Vector3 _modelSize;
        private readonly float _maxSpeed;
        private readonly float _deltaSpeed;
        private readonly float _rotationSpeed;
        private Vector3 _currentVelocity;
        private Vector2 _input;

        public event Action Die;
        public event Action<float> SpeedChanged;
        public event Action<Vector3> RotationChanged;
        public event Action<Vector3> PositionChanged;

        public Ship(Rigidbody rigidBody, Vector3 modelSize, ShipConfig shipConfigure)
        {
            _rigidBody = rigidBody;
            _modelSize = modelSize;
            _maxSpeed = shipConfigure.MaxSpeed;
            _deltaSpeed = shipConfigure.DeltaSpeed;
            _rotationSpeed = shipConfigure.RotationSpeed;
        }

        public void SetInput(Vector2 inputData)
        {
            _input = inputData;
        }

        public void LetsDie()
        {
            Die?.Invoke();
        }

        void IFixedTickable.FixedTick()
        {
            if (_input.y > 0)
                Move();

            if (_input.x != 0)
                Rotate(_input.x);
        }

        void IInitializable.Initialize()
        {
            PositionChanged?.Invoke(_rigidBody.transform.position);
            SpeedChanged?.Invoke(_rigidBody.velocity.magnitude);
            RotationChanged?.Invoke(_rigidBody.rotation.eulerAngles);
        }

        private void Move()
        {
            Vector3 moveDirection = _rigidBody.rotation * Vector3.up;
            _currentVelocity = _rigidBody.velocity;
            _currentVelocity += moveDirection * _deltaSpeed;
            _currentVelocity = Vector3.ClampMagnitude(_currentVelocity, _maxSpeed);
            _rigidBody.velocity = _currentVelocity;

            if (TryWrap(out Vector3 newPosition))
                _rigidBody.transform.position = newPosition;

            SpeedChanged?.Invoke(_rigidBody.velocity.magnitude);
            PositionChanged?.Invoke(_rigidBody.transform.position);
        }

        private void Rotate(float rotateDirection)
        {
            Vector3 lastRotation = _rigidBody.rotation.eulerAngles;
            Vector3 deltaRotation;

            deltaRotation = Vector3.forward * _rotationSpeed * -rotateDirection * Time.fixedDeltaTime;
            lastRotation += deltaRotation;

            _rigidBody.rotation = Quaternion.Euler(lastRotation);

            RotationChanged?.Invoke(_rigidBody.rotation.eulerAngles);
        }

        private bool TryWrap(out Vector3 newPosition)
        {
            newPosition = _rigidBody.transform.position;

            Vector3 absPosition = new Vector3();

            absPosition.x = Mathf.Abs(_rigidBody.transform.position.x);
            absPosition.y = Mathf.Abs(_rigidBody.transform.position.y);

            if (absPosition.x > Utilities.ScreenBounds.x + _modelSize.x)
                newPosition.x = -_rigidBody.transform.position.x;

            if (absPosition.y > Utilities.ScreenBounds.y + _modelSize.y)
                newPosition.y = -_rigidBody.transform.position.y;

            if (newPosition != _rigidBody.transform.position)
                return true;

            return false;
        }
    }
}
