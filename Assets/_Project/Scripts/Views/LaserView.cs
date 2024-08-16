namespace Assets.Models
{
    public class LaserView : BaseView<Laser>, IGunShot
    {
        public DamageType DamageType => DamageType.Laser;

        public override void Initialize(Laser model)
        {
            base.Initialize(model);
            Model.Died += Die;
        }

        private void Die(Laser laser)
        {
            gameObject.SetActive(false);
        }
    }
}