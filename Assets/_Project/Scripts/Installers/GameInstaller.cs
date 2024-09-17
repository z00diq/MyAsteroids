using Assets.Infrastructure;
using Assets.Models;
using Assets.Scripts;
using Assets.Views;
using UnityEngine;
using Zenject;

namespace Assets.Installers
{
    public sealed class GameInstaller : MonoInstaller
    {
        [SerializeField] private ShipUI _shipUI;
        [SerializeField] private ShipView _prefab;
        [SerializeField] private Vector3 _spawnPosition = Vector3.zero;

        public override void InstallBindings()
        {
            Vector2 ScreenSize = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
            Utilities.ScreenBounds = ScreenSize;

            Container.BindInterfacesAndSelfTo<AsteroidFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<UFOFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<BulletSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<LaserFireController>().AsSingle();
            Container.BindFactory<float, Transform, Vector3, Bullet, Bullet.Factroy>();
            Container.Bind<FireConfig>().AsSingle();
            Container.Bind<ShipView>().FromComponentInNewPrefab(_prefab).AsSingle();
            Container.BindInterfacesAndSelfTo<ShipKeyboardController>().AsSingle();
            Container.BindInterfacesAndSelfTo<Ship>().FromFactory<Ship, ShipFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<ShipFire>().FromFactory<ShipFire, ShipFactory>().AsSingle();
            Container.Bind<ShipUI>().FromComponentInNewPrefab(_shipUI).AsSingle();
            Container.BindInterfacesAndSelfTo<ShipUIController>().AsSingle();
        }
    }
}
