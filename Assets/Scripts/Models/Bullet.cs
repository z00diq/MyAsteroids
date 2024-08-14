using Assets.Scripts;
using Assets.Views;
using System;
using UnityEngine;

namespace Assets.Models
{
    public class Bullet:IUpdatable
    {
        private BulletView _view;
        private Vector3 _position;
        private float _bulletSpeed;

        public event Action<Vector3> PositionChanged;
        public event Action<Bullet> OutFromBounds;

        public Bullet(float bulletSpeed)
        {
            _bulletSpeed = bulletSpeed;
        }

        public GameObject ViewGameObject => _view.gameObject;

        public void Update()
        {
            Move();
        }

        internal void Initialize(BulletView bulletView)
        {
            _view = bulletView;
            bulletView.Initialize(this);
            _view.Died += Died;
            _position = _view.transform.position;
        }

        public void SetPosition(Vector3 firePosition)
        {
            _position = firePosition;
        }

        private void Move()
        {
            Vector3 deltaMove = _view.transform.up;
            deltaMove *= _bulletSpeed * Time.deltaTime;
            _position += deltaMove;
            PositionChanged?.Invoke(_position);

            if (Extensions.IsPositionTooFar(_position, _view))
                OutFromBounds?.Invoke(this);
        }

        private void Died()
        {
            Game.Instance.RemoveFromUpdatable(this);
        }
    }
}
