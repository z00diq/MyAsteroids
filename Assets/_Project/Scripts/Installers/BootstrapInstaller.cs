using Zenject;

public sealed class BootstrapInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Bootstrap>().AsSingle();
    }
}
