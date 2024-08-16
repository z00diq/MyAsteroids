using Assets.Models;
using Assets.Scripts;
using Assets.Views;
using UnityEngine;

public class ShipFactory
{
    private ShipConfigure _shipConfigure;
    private BulletsFireConfigure _bulletsConfiguration;
    private LaserFireConfigure _laserConfiguration;
    private Vector3 _firePosition;
    private GameLoop _gameLoop;

    public Transform ShipTransform { get; private set; }
    public Ship Ship { get; private set; }
    public ShipFire ShipFire {get; private set;}


    public ShipFactory(ShipConfigure shipConfigure, BulletsFireConfigure bulletsFireConfigure, LaserFireConfigure laserFireConfigure, GameLoop gameLoop)
    {
        _shipConfigure = shipConfigure;
        _bulletsConfiguration = bulletsFireConfigure;
        _laserConfiguration = laserFireConfigure;
        _gameLoop = gameLoop;
    }

    public void CreateShip(Vector3 spawnPosition)
    {
        ShipView shipView = Object.Instantiate(_shipConfigure.Prefab, spawnPosition, Quaternion.identity);
        ShipTransform = shipView.transform;
        FireConfig fireConfig = new FireConfig(shipView);
        ShipFire = new ShipFire(fireConfig, _bulletsConfiguration, _laserConfiguration, _gameLoop);
        Ship = new Ship(shipView.GetComponent<Rigidbody>(), shipView.Size, _shipConfigure);
        shipView.Initialize(Ship);
    }
}

public class FireConfig
{
    private ShipView _shipView;
    public FireConfig(ShipView view)
    {
        _shipView = view;
    }

    public Transform Transform => _shipView.transform;

    public Vector3 FirePosition => _shipView.FirePosition;
}