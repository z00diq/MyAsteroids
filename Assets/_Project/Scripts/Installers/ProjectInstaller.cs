using Assets._Project.Scripts.Remotes;
using Assets.Infrastructure;
using Zenject;

public sealed class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SceneSecretary>().AsSingle();
        Container.Bind<RemoteAnalytics>().To<FireBaseAnalytics>().AsSingle();
    }
}
