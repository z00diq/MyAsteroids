namespace Assets.Models
{
    public class LaserView : BaseView<Laser>
    {
        public override void Initialize(Laser model)
        {
            base.Initialize(model);
            RenderSize.Died += Die;
        }

        private void Die(Laser laser)
        {
            gameObject.SetActive(false);
        }
    }
}