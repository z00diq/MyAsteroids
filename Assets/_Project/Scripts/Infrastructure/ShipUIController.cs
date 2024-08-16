using Assets.Models;

namespace Assets.Scripts
{
    public class ShipUIController : IStartable
    {
        private Ship _ship;
        private ShipFire _shipFire;
        private ShipUI _shipUI;

        public ShipUIController(Ship ship, ShipFire shipFire, ShipUI shipUI)
        {
            _ship = ship;
            _shipFire = shipFire;
            _shipUI = shipUI;
        }

        public void Start()
        {
            Bind();
        }

        private void Bind()
        {
            _ship.PositionChanged += _shipUI.OnPositionChanged;
            _ship.RotationChanged += _shipUI.OnRotationChanged;
            _ship.SpeedChanged += _shipUI.OnSpeedChanged;
            _ship.Die += _shipUI.OnGameOver;
            _shipFire.LaserCountChanged += _shipUI.OnLaserCountChanged;
            _shipFire.LaserReloadTimeChanged += _shipUI.OnLaserReloadTimeChanged;
        }

        private void Unbind()
        {
            _ship.PositionChanged -= _shipUI.OnPositionChanged;
            _ship.RotationChanged -= _shipUI.OnRotationChanged;
            _ship.SpeedChanged -= _shipUI.OnSpeedChanged;
            _ship.Die -= _shipUI.OnGameOver;
            _shipFire.LaserCountChanged -= _shipUI.OnLaserCountChanged;
            _shipFire.LaserReloadTimeChanged -= _shipUI.OnLaserReloadTimeChanged;
        }
    }
}
