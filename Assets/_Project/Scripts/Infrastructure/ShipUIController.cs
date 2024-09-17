using Assets.Models;
using Assets.Scripts;
using System;
using Zenject;

namespace Assets.Infrastructure
{
    public class ShipUIController : IInitializable, IDisposable
    {
        private readonly Ship _ship;
        private readonly LaserFireController _laserFire;
        private readonly ShipUI _shipUI;

        public ShipUIController(Ship ship, LaserFireController laserFire, ShipUI shipUI)
        {
            _ship = ship;
            _laserFire = laserFire;
            _shipUI = shipUI;
        }

        [Inject]
        void IInitializable.Initialize()
        {
            Bind();
        }

        void IDisposable.Dispose()
        {
            Unbind();
        }

        private void Bind()
        {
            _ship.PositionChanged += _shipUI.OnPositionChanged;
            _ship.RotationChanged += _shipUI.OnRotationChanged;
            _ship.SpeedChanged += _shipUI.OnSpeedChanged;
            _ship.Die += _shipUI.OnGameOver;
            _laserFire.LaserCountChanged += _shipUI.OnLaserCountChanged;
            _laserFire.LaserReloadTimeChanged += _shipUI.OnLaserReloadTimeChanged;
        }

        private void Unbind()
        {
            _ship.PositionChanged -= _shipUI.OnPositionChanged;
            _ship.RotationChanged -= _shipUI.OnRotationChanged;
            _ship.SpeedChanged -= _shipUI.OnSpeedChanged;
            _ship.Die -= _shipUI.OnGameOver;
            _laserFire.LaserCountChanged -= _shipUI.OnLaserCountChanged;
            _laserFire.LaserReloadTimeChanged -= _shipUI.OnLaserReloadTimeChanged;
        }
    }
}
