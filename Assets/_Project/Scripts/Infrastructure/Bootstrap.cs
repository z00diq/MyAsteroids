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
using Assets._Project.Scripts.IAP;
using System;

public class Bootstrap: IInitializable,IDisposable
{
    private IRemoteConfig _remoteConfig;
    private IInAppPurchase _inAppPurchase;
    private SceneSecretary _sceneSecretary;
    private RemoteAnalytics _analytics;
    private Ads _ads;
    private bool disposedValue;

    public Bootstrap(IRemoteConfig remoteConfig, SceneSecretary sceneSecretary, RemoteAnalytics analytics,Ads ads,IInAppPurchase iap)
    {
        _remoteConfig = remoteConfig;
        _sceneSecretary = sceneSecretary;
        _analytics = analytics;
        _ads=ads;
        _inAppPurchase = iap;
    }

    async void IInitializable.Initialize()
    {
        await _remoteConfig.FetchDataAsync().
            ContinueWith(task =>_remoteConfig.ActivateDataAsync(task).
            ContinueWith(task => _remoteConfig.LoadDataAsync(task)));

        await _analytics.InitializeAsync();
        await _inAppPurchase.Initialize();
        await _ads.InitializeAds();
        await _ads.ShowInterstitial();

        _inAppPurchase.PurchaseComplite += _ads.DisableAds;
        _sceneSecretary.ToGamScene();
    }

    public void Dispose()
    {
        _inAppPurchase.PurchaseComplite -= _ads.DisableAds;
    }
}
