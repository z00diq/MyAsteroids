using Assets._Project.Scripts.Remotes;
using System.Threading.Tasks;
using Assets.Configurations;
using Assets.Infrastructure;
using Assets.Installers;
using Zenject;
using Firebase.Extensions;
using Firebase;

public class Bootstrap: IInitializable
{
    private IRemoteConfig _remoteConfig;
    private SceneSecretary _sceneSecretary;
    private FirebaseApp app;

    public Bootstrap(IRemoteConfig remoteConfig, SceneSecretary sceneSecretary)
    {
        _remoteConfig = remoteConfig;
        _sceneSecretary = sceneSecretary;
    }

    async void IInitializable.Initialize()
    {
        await _remoteConfig.FetchDataAsync().
            ContinueWith(task =>_remoteConfig.ActivateDataAsync(task).
            ContinueWith(task => _remoteConfig.LoadDataAsync(task)));

        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;

                Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLogin,new Firebase.Analytics.Parameter[] 
                {
                    new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterValue, 1),
                    new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterValue, "test")
                });
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

        _sceneSecretary.ToGamScene();
    }
}
