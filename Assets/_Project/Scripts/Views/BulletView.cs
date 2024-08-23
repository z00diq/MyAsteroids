using Assets.Models;

namespace Assets.Views
{
    public class BulletView : BaseView<Bullet>, IGunShot
    {
        public DamageType DamageType => DamageType.Bullet;

        public override void Initialize(Bullet bullet)
        {
            base.Initialize(bullet);

            Model.Enabled += Enabled;
        }

        private void Enabled(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void OnDestroy()
        {
            Model.Enabled -= Enabled;
        }
    }
}