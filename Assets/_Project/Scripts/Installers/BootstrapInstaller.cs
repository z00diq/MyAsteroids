using Assets._Project.Scripts.Remotes;
using Zenject;

public sealed class BootstrapInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<Bootstrap>().AsSingle();
        Container.Bind<IRemoteConfig>().To<FireBaseRemoteConfig>().AsSingle();
    }
}
