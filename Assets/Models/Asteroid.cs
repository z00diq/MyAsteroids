using Assets.Scripts;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Models
{
    public class Asteroid: IUpdatable
    {
        private AsteroidView _view;
        private Vector3 _position;
        private Vector3 _moveVector;
        private float _minSpeed;
        private float _maxSpeed;
        private float _speed;
        private float _tooFarDistance;

        public event Action<Vector3> Moved;
        public event Action<Asteroid> OutFromBounds;
        
        public GameObject ViewGameObject => _view.gameObject;

        public Asteroid(float minSpeed, float maxSpeed, float tooFarDistance) 
        {
            _minSpeed = minSpeed;
            _maxSpeed = maxSpeed;
            _tooFarDistance = tooFarDistance;
        }

        public void Initialize(AsteroidView view)
        {
            view.Initialize(this);
            _view = view;
            _view.TriggerDetected += TriggerDetected;
            _position = view.transform.position;
            CalculateMoveSettings();
        }

        public void Update()
        {
            Move();
        }

        public void SetPosition(Vector3 position)
        {
            _position = position;
            CalculateMoveSettings();
            Moved?.Invoke(_position);
        }

        private void Move()
        {
            Vector3 deltaMoveVector =_moveVector * Time.deltaTime;
            _position += deltaMoveVector;
            Moved?.Invoke(_position);

            if (Extensions.IsPositionTooFar(_position, _view,_tooFarDistance))
                OutFromBounds?.Invoke(this);
        }

        private void CalculateMoveSettings()
        {
            Vector3 target = new Vector3(Game.Instance.ScreenBounds.x / 2, Game.Instance.ScreenBounds.y / 2);
            Vector3 direction = (target - _position).normalized;
            float speed = Random.Range(_minSpeed, _maxSpeed);
            _moveVector = direction * speed;
        }

        private void TriggerDetected(Collider collider)
        {
            if (collider.TryGetComponent(out IInteractable interactable) == false)
                return;

            interactable.Do();

        }
    }
}
