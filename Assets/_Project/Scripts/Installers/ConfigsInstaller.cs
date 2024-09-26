using Assets._Project.Scripts.Ads;
using Assets.Configurations;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Installers
{
    [CreateAssetMenu(menuName = "Configs/Mains/Game", fileName = "Config (Main Game Settings)")]
    public sealed class ConfigsInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private AsteroidConfig _asteroidsConfig;
        [SerializeField] private ParticleAsteroidConfig _smallAsteroidConfig;
        [SerializeField] private UFOConfig _ufoConfig;
        [SerializeField] private ShipConfig _shipConfig;
        [SerializeField] private LaserFireConfig _laserConfig;
        [SerializeField] private BulletsFireConfig _bulletsConfig;
        [SerializeField] private AdsConfig _adsConfig;

        public override void InstallBindings()
        {
            Container.Bind<AsteroidConfig>().FromInstance(_asteroidsConfig).AsSingle();
            Container.Bind<ParticleAsteroidConfig>().FromInstance(_smallAsteroidConfig).AsSingle();
            Container.Bind<UFOConfig>().FromInstance(_ufoConfig).AsSingle();
            Container.Bind<ShipConfig>().FromInstance(_shipConfig).AsSingle();
            Container.Bind<LaserFireConfig>().FromInstance(_laserConfig).AsSingle();
            Container.Bind<BulletsFireConfig>().FromInstance(_bulletsConfig).AsSingle();
            Container.Bind<AdsConfig>().FromInstance(_adsConfig).AsSingle();
        }
    }
}
