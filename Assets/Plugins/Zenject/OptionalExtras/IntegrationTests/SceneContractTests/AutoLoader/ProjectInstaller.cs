namespace Zenject.Tests.AutoLoadSceneTests
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Qux>().AsSingle();
        }
    }
}
