using Assets.Models;
using UnityEngine;
using Zenject;

namespace Assets.Infrastructure
{
    public class ShipKeyboardController : ITickable
    {
        private const string Horizontal = nameof(Horizontal);
        private const string Vertical = nameof(Vertical);

        private const KeyCode fireButton = KeyCode.F;
        private const KeyCode altFireButton = KeyCode.G;

        private readonly Ship _ship;
        private readonly ShipFire _shipFire;
        private Vector2 _inputData = Vector2.zero;

        public ShipKeyboardController(Ship ship, ShipFire shipFire)
        {
            _ship = ship;
            _shipFire = shipFire;
        }

        void ITickable.Tick()
        {
            _inputData.x = Input.GetAxisRaw(Horizontal);
            _inputData.y = Input.GetAxisRaw(Vertical);

            _ship.SetInput(_inputData);

            if (Input.GetKey(fireButton))
                _shipFire.BulletFire();

            if (Input.GetKey(altFireButton))
                _shipFire.LaserFire();
        }
    }
}
