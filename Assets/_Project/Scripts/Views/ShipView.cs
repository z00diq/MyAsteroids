using Assets.Models;
using UnityEngine;

namespace Assets.Views
{
    [RequireComponent(typeof(Rigidbody))]
    public class ShipView : BaseView<Ship>
    {
        [SerializeField] private Transform _bulletSpawn;
        public Vector3 FirePosition => _bulletSpawn.position;


        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.TryGetComponent(out EnemyView view)) 
                Model.LetsDie();
        }
    }
}
