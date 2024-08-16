using Assets.Infrastructure;
using Assets.Scripts;
using System;
using UnityEngine;

namespace Assets.Models
{
    public class Ship : IFixedUpdatable, IStartable
    {
        private Rigidbody _rigidBody;
        private Vector3 _modelSize;
        private Vector3 _currentVelocity;
        private float _maxSpeed;
        private float _deltaSpeed;
        private float _rotationSpeed;

        private float _rotateDirection;
        private bool _isMoveing = false;
        private bool _isRotating = false;

        public event Action Die;
        public event Action<float> SpeedChanged;
        public event Action<Vector3> RotationChanged;
        public event Action<Vector3> PositionChanged;

        public Ship(Rigidbody rigidBody, Vector3 modelSize,ShipConfigure shipConfigure)
        {
            _rigidBody = rigidBody;
            _modelSize = modelSize;
            _maxSpeed = shipConfigure.MaxSpeed;
            _deltaSpeed = shipConfigure.DeltaSpeed;
            _rotationSpeed = shipConfigure.RotationSpeed;
        }

        private void InputChanged(Vector2 input)
        {
            if(input.y>0)
                _isMoveing = true;

            if (input.x != 0)
            {
                _rotateDirection = input.x;
                _isRotating = true;
            }
        }

        public void LetsMove()
        {
            _isMoveing = true;
        }

        public void LetsRotate(float rotateDirection)
        {
            _rotateDirection = rotateDirection;
            _isRotating = true; ;
        }

        public void Start()
        {
            PositionChanged?.Invoke(_rigidBody.transform.position);
            SpeedChanged?.Invoke(_rigidBody.velocity.magnitude);
            RotationChanged?.Invoke(_rigidBody.rotation.eulerAngles);
        }

        public void FixedUpdate()
        {
            if (_isMoveing)
            {
                Move();
                _isMoveing = false;
            }

            if (_isRotating)
            {
                Rotate();
                _isRotating = false;
            }
        }

        public void LetsDie()
        {
            Die?.Invoke();
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

            PositionChanged?.Invoke(_rigidBody.transform.position);
            SpeedChanged?.Invoke(_rigidBody.velocity.magnitude);
        }

        private void Rotate()
        {
            Vector3 lastRotation = _rigidBody.rotation.eulerAngles;
            Vector3 deltaRotation;

            deltaRotation = Vector3.forward * _rotationSpeed * -_rotateDirection * Time.fixedDeltaTime;
            lastRotation += deltaRotation;

            _rigidBody.rotation = Quaternion.Euler(lastRotation);

            RotationChanged?.Invoke(_rigidBody.rotation.eulerAngles);
        }

        private  bool TryWrap(out Vector3 newPosition)
        {
            newPosition = _rigidBody.transform.position;
           
            Vector3 absPosition = new Vector3();

            absPosition.x = Mathf.Abs(_rigidBody.transform.position.x);
            absPosition.y = Mathf.Abs(_rigidBody.transform.position.y);

            if (absPosition.x > Game.Instance.ScreenBounds.x + _modelSize.x)
                newPosition.x = -_rigidBody.transform.position.x;
        
            if(absPosition.y > Game.Instance.ScreenBounds.y + _modelSize.y)
                newPosition.y = -_rigidBody.transform.position.y;

            if (newPosition != _rigidBody.transform.position)
                return true;

            return false;
        }
    }
}
