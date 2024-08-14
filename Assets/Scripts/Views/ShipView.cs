using Assets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Views
{
    [RequireComponent(typeof(Rigidbody))]
    public class ShipView : EnemyView, IEnnemyInteractable
    {
        [SerializeField] private Transform _bulletSpawn;

        private Ship _model;

        public event Action GameOver;

        public Vector3 FirePosition => _bulletSpawn.position;
        public Rigidbody RigidBody { get; private set;}

        public void Initilize(Ship ship)
        {
            _model = ship;
            RigidBody = GetComponent<Rigidbody>();
        }

        public void Impact(Destroyable enemy)
        {
            GameOver?.Invoke();
        }
    }
}
