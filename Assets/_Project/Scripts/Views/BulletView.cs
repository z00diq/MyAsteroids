using Assets.Models;
using System;
using UnityEngine;

namespace Assets.Views
{
    public class BulletView : BaseView<Bullet>
    {
        public override void Initialize(Bullet bullet)
        {
            base.Initialize(bullet);
            bullet.PositionChanged += PositionChanged;
        }

        private void PositionChanged(Vector3 position)
        {
            transform.position = position;
        }
    }
}