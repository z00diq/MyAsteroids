using Assets.Models;
using System;
using UnityEngine;

namespace Assets.Views
{
    public class BulletView : View, IEnnemyInteractable
    {
        private Bullet _model;

        public event Action Died;

        public void Impact(Destroyable enemy)
        {
            if(enemy is Asteroid)
                ((Asteroid)enemy).Destroy();
            else
                enemy.Destroy();

            Died?.Invoke();
            Destroy(gameObject);
        }

        public void Initialize(Bullet bullet)
        {
            _model = bullet;
            _model.PositionChanged += PositionChanged;
        }

        private void PositionChanged(Vector3 position)
        {
            transform.position = position;
        }
    }
}