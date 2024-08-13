using Assets.Scripts;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Models
{
    public class Asteroid: BaseEnemy
    {
        public event Action<Asteroid> OutFromBounds;

        public Asteroid(float minSpeed, float maxSpeed, float tooFarDistance) : base(minSpeed, maxSpeed, tooFarDistance)
        {
        }

        public override void SetPosition(Vector3 position)
        {
            Position = position;
            CalculateMoveSettings();
            base.SetPosition(position);
        }

        public new void Destroy()
        {
            Explose();
            base.Destroy();
        }

        protected override void CalculateMoveSettings()
        {
            Vector3 target = new Vector3(Game.Instance.ScreenBounds.x / 2, Game.Instance.ScreenBounds.y / 2);
            Vector3 direction = (target - Position).normalized;
            Speed = Random.Range(MinSpeed, MaxSpeed);
            MoveVector = direction * Speed;
        }

        

        private void Explose()
        {
            int pieceCount = Random.Range(1, 6);

            for (int i = 0; i < pieceCount; i++)
            {
                EnemyView miniAsteroidView = AsteroidView.Instantiate(View, ViewGameObject.transform.position, Quaternion.identity);
                miniAsteroidView.transform.localScale = View.transform.localScale * 0.3f;
                
                BaseEnemy miniAsteroid = new BaseEnemy(MaxSpeed, MaxSpeed * 1.5f,TooFarDistance);
                miniAsteroid.Initialize(miniAsteroidView);
                Game.Instance.AddToUpdatable(miniAsteroid);
            }
        }

        protected override void TrySendCallback()
        {
            if (Extensions.IsPositionTooFar(Position, View, TooFarDistance))
                OutFromBounds?.Invoke(this);
        }
    }
}
