using Assets.Infrastructure;
using Assets.Views;
using UnityEngine;

namespace Assets.Models
{
    public class Ship : IFixedUpdatable
    {
        private ShipView _view;
        private InputSystem _input;
        private ShipFire _shipFire;
        private Vector3 _currentSpeed;
        private float _maxSpeed;
        private float _deltaSpeed;
        private float _rotationSpeed;
        private float _rotateDirection;
        private bool _isMoveing = false;
        private bool _isRotating = false;

        public Ship(float maxSpeed, float deltaSpeed, float rotationSpeed, InputSystem inputSystem, ShipFire shipFire)
        {
            _maxSpeed = maxSpeed;
            _deltaSpeed = deltaSpeed;
            _rotationSpeed = rotationSpeed;
            _input = inputSystem;
            _shipFire = shipFire;
            _input.InputChanged += InputChanged;
            _input.FireButtonPressed += _shipFire.FireBullet;
            _input.AltFireButtonPressed += _shipFire.LaserFire;
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

        public void Initilize(ShipView shipView)
        {
            _view= shipView;
            shipView.Initilize(this);
        }

        private void Move()
        {
            Vector3 moveDirection = Vector3.up;
            Vector3 deltaSpeed = new Vector3(0f, _deltaSpeed, 0f);
            moveDirection = _view.RigidBody.rotation * moveDirection;
            moveDirection *= _deltaSpeed;
            _currentSpeed = _view.RigidBody.velocity;
            _currentSpeed += moveDirection;
            _currentSpeed = Vector3.ClampMagnitude(_currentSpeed, _maxSpeed);
            _view.RigidBody.velocity = _currentSpeed;
        }

        private void Rotate()
        {
            Vector3 lastRotation = _view.RigidBody.rotation.eulerAngles;
            Vector3 deltaRotation;

            deltaRotation = Vector3.forward * _rotationSpeed * -_rotateDirection * Time.fixedDeltaTime;
            lastRotation += deltaRotation;

            _view.RigidBody.rotation = Quaternion.Euler(lastRotation);
        }
    }
}
