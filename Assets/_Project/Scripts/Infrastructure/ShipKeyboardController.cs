using Assets.Models;
using Assets.Scripts;
using UnityEngine;

namespace Assets.Infrastructure
{
    public class ShipKeyboardController : IUpdatable
    {
        private const string Horizontal = nameof(Horizontal);
        private const string Vertical = nameof(Vertical);

        private const KeyCode fireButton = KeyCode.F;
        private const KeyCode altFireButton = KeyCode.G;

        private Ship _ship;
        private ShipFire _shipFire;
        private Vector2 _inputData = Vector2.zero;

        public ShipKeyboardController(Ship ship, ShipFire shipFire)
        {
            _ship = ship;
            _shipFire = shipFire;
        }

        public void Update()
        {
            _inputData.x = Input.GetAxisRaw(Horizontal);
            _inputData.y = Input.GetAxisRaw(Vertical);

            if (_inputData.y > 0)
                _ship.LetsMove();

            if (_inputData.x != 0)
                _ship.LetsRotate(_inputData.x);

            if (Input.GetKey(fireButton))
                _shipFire.FireBullet();

            if (Input.GetKey(altFireButton))
                _shipFire.LaserFire();
        }
    }
}
