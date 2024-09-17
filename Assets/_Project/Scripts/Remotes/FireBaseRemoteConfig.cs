using Assets.Configurations;
using Assets.Installers;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Project.Scripts.Remotes
{
    public class FireBaseRemoteConfig : IRemoteConfig
    {
        private FirebaseRemoteConfig _remoteConfig;
        private AsteroidConfig _asteroidConfig;
        private BulletsFireConfig _bulletsConfig;
        private UFOConfig _ufoConfig;
        private ShipConfig _shipConfig;
        private ParticleAsteroidConfig _smallAsteroidConfig;
        private LaserFireConfig _laserConfig;

        public FireBaseRemoteConfig(AsteroidConfig asteroidConfig, BulletsFireConfig bulletsConfig, UFOConfig ufoConfig, ShipConfig shipConfig, ParticleAsteroidConfig smallAsteroidConfig, LaserFireConfig laserConfig)
        {
            _asteroidConfig = asteroidConfig;
            _bulletsConfig = bulletsConfig;
            _ufoConfig = ufoConfig;
            _shipConfig = shipConfig;
            _smallAsteroidConfig = smallAsteroidConfig;
            _laserConfig = laserConfig;
        }

        public Task FetchDataAsync()
        {
            Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
            return fetchTask;
        }

        public Task ActivateDataAsync(Task fetchTask)
        {
            if (!fetchTask.IsCompleted)
            {
                Debug.LogError("Retrieval hasn't finished.");
                return null;
            }

            _remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            ConfigInfo info = _remoteConfig.Info;

            if (info.LastFetchStatus != LastFetchStatus.Success)
            {
                Debug.LogError($"{nameof(fetchTask)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
                return null;
            }

            Task activateTask = _remoteConfig.ActivateAsync();
            return activateTask;
        }

        public Task LoadDataAsync(Task activateTask)
        {
            if (!activateTask.IsCompleted)
            {
                Debug.LogError("Retrieval hasn't finished.");
                return null;
            }

            ConfigInfo info = _remoteConfig.Info;

            if (info.LastFetchStatus != LastFetchStatus.Success)
            {
                Debug.LogError($"{nameof(activateTask)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
                return null;
            }

            Task getDataTask = Task.WhenAll(new Task[]
            {
                Task.Run(()=>JsonUtility.FromJsonOverwrite(_remoteConfig.GetValue(nameof(AsteroidConfig)).StringValue, _asteroidConfig)),
                Task.Run(()=> JsonUtility.FromJsonOverwrite(_remoteConfig.GetValue(nameof(BulletsFireConfig)).StringValue, _bulletsConfig)),
                Task.Run(()=>JsonUtility.FromJsonOverwrite(_remoteConfig.GetValue(nameof(UFOConfig)).StringValue, _ufoConfig)),
                Task.Run(()=>JsonUtility.FromJsonOverwrite(_remoteConfig.GetValue(nameof(ShipConfig)).StringValue, _shipConfig)),
                Task.Run(()=>JsonUtility.FromJsonOverwrite(_remoteConfig.GetValue(nameof(ParticleAsteroidConfig)).StringValue, _smallAsteroidConfig)),
                Task.Run(()=> JsonUtility.FromJsonOverwrite(_remoteConfig.GetValue(nameof(LaserFireConfig)).StringValue, _laserConfig))
            });

            return getDataTask;
        }
    }
}
