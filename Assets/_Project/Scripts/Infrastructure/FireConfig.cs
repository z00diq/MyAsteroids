using Assets.Views;
using UnityEngine;

namespace Assets.Infrastructure
{
    public class FireConfig
    {
        private readonly ShipView _shipView;
        public FireConfig(ShipView view)
        {
            _shipView = view;
        }

        public Transform Transform => _shipView.transform;

        public Vector3 FirePosition => _shipView.FirePosition;
    }
}
