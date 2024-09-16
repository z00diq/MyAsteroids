using Assets.Infrastructure;
using Zenject;

public sealed class SceneManagmentInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SceneSecretary>().AsSingle();
    }
}
