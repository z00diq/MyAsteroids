using Assets.Configurations;
using Assets.Infrastructure;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class Bootstrap : MonoBehaviour
{
    private FirebaseRemoteConfig _remoteConfig;
    private SceneSecretary _sceneSecretary;
    private AsteroidConfig _asteroidConfig;
    private BulletsFireConfig _bulletsConfig;
    private UFOConfig _ufoConfig;
    private ShipConfig _shipConfig;
    private ParticleAsteroidConfig _smallAsteroidConfig;
    private LaserFireConfig _laserConfig;

    [Inject]
    public void Construct(DiContainer container)
    {
        _sceneSecretary = container.Resolve<SceneSecretary>();
        _asteroidConfig = container.Resolve<AsteroidConfig>();
        _bulletsConfig = container.Resolve<BulletsFireConfig>();
        _ufoConfig = container.Resolve<UFOConfig>();
        _shipConfig = container.Resolve<ShipConfig>();
        _smallAsteroidConfig = container.Resolve<ParticleAsteroidConfig>();
        _laserConfig = container.Resolve<LaserFireConfig>();
    }

    private void Start()
    {
        FetchDataAsync();
    }

    private Task FetchDataAsync()
    {
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task fetchTask)
    {
        if (!fetchTask.IsCompleted)
        {
            Debug.LogError("Retrieval hasn't finished.");
            return;
        }

        _remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        ConfigInfo info = _remoteConfig.Info;

        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
            Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            return;
        }

        Task task = _remoteConfig.ActivateAsync();
        task.ContinueWithOnMainThread(TakeData);
    }

    private void TakeData(Task task)
    {
        if (!task.IsCompleted)
        {
            Debug.LogError("Retrieval hasn't finished.");
            return;
        }

        ConfigInfo info = _remoteConfig.Info;

        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
            Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            return;
        }

        JsonUtility.FromJsonOverwrite(_remoteConfig.GetValue(nameof(AsteroidConfig)).StringValue, _asteroidConfig);
        JsonUtility.FromJsonOverwrite(_remoteConfig.GetValue(nameof(BulletsFireConfig)).StringValue, _bulletsConfig);
        JsonUtility.FromJsonOverwrite(_remoteConfig.GetValue(nameof(UFOConfig)).StringValue, _ufoConfig);
        JsonUtility.FromJsonOverwrite(_remoteConfig.GetValue(nameof(ShipConfig)).StringValue, _shipConfig);
        JsonUtility.FromJsonOverwrite(_remoteConfig.GetValue(nameof(ParticleAsteroidConfig)).StringValue, _smallAsteroidConfig);
        JsonUtility.FromJsonOverwrite(_remoteConfig.GetValue(nameof(LaserFireConfig)).StringValue, _laserConfig);

        _sceneSecretary.ReloadMainSceneAsSingle();
        _sceneSecretary.CloseBootstrapScene();
    }
}
