using Assets._Project.Scripts.Remotes;
using System.Threading.Tasks;
using Assets.Configurations;
using Assets.Infrastructure;
using Assets.Installers;
using Zenject;

public class Bootstrap: IInitializable
{
    private IRemoteConfig _remoteConfig;
    private SceneSecretary _sceneSecretary;

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

        _sceneSecretary.ToGamScene();
    }
}
