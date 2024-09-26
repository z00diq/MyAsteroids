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
using Assets._Project.Scripts.Ads;

public class Bootstrap: IInitializable
{
    private IRemoteConfig _remoteConfig;
    private SceneSecretary _sceneSecretary;
    private RemoteAnalytics _analytics;
    private Ads _ads;

    public Bootstrap(IRemoteConfig remoteConfig, SceneSecretary sceneSecretary, RemoteAnalytics analytics,Ads ads)
    {
        _remoteConfig = remoteConfig;
        _sceneSecretary = sceneSecretary;
        _analytics = analytics;
        _ads=ads;
    }

    async void IInitializable.Initialize()
    {
        await _remoteConfig.FetchDataAsync().
            ContinueWith(task =>_remoteConfig.ActivateDataAsync(task).
            ContinueWith(task => _remoteConfig.LoadDataAsync(task)));

        await _analytics.InitializeAsync();

        await _ads.InitializeAds();
        await _ads.ShowInterstitial();

        _sceneSecretary.ToGamScene();
    }
}
