namespace Assets.Models
{
    public class LaserView : View, IEnnemyInteractable
    {
        private Laser _model;

        public void Impact(Destroyable enemy)
        {
            enemy.Destroy();
        }

        public void Initialize(Laser model)
        {
            _model = model;
            _model.Died += Die;
        }

        private void Die(Laser laser)
        {
            gameObject.SetActive(false);
        }
    }
}