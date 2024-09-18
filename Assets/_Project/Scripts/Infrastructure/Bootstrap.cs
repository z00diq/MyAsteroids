using Assets._Project.Scripts.Remotes;
using System.Threading.Tasks;
using Assets.Configurations;
using Assets.Infrastructure;
using Assets.Installers;
using Zenject;
using Firebase.Extensions;
using Firebase;
using Firebase.Analytics;
using UnityEngine.Analytics;

public class Bootstrap: IInitializable
{
    private IRemoteConfig _remoteConfig;
    private SceneSecretary _sceneSecretary;
    private RemoteAnalytics _analytics;

    public Bootstrap(IRemoteConfig remoteConfig, SceneSecretary sceneSecretary, RemoteAnalytics analytics)
    {
        _remoteConfig = remoteConfig;
        _sceneSecretary = sceneSecretary;
        _analytics = analytics;
    }

    async void IInitializable.Initialize()
    {
        await _remoteConfig.FetchDataAsync().
            ContinueWith(task =>_remoteConfig.ActivateDataAsync(task).
            ContinueWith(task => _remoteConfig.LoadDataAsync(task)));

        await _analytics.InitializeAsync();

        _sceneSecretary.ToGamScene();
    }
}
