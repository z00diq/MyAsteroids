using Assets.Models;
using System;

namespace Assets.Views
{
    public class LaserView : BaseView<Laser>, IGunShot
    {
        public DamageType DamageType => DamageType.Laser;

        public override void Initialize(Laser model)
        {
            base.Initialize(model);
            Model.StartFiering += Active;
            Model.EndFireing += Unactive;

        }

        private void Active()
        {
            gameObject.SetActive(true);
        }

        private void Unactive()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Model.EndFireing -= Unactive;
        }
    }
}