using Assets.Models;
using UnityEngine;

namespace Assets.Configurations
{
    [CreateAssetMenu(menuName = "Configs/Ship/Fire/Bullets", fileName = "Ship Bullet Fire Configuration")]
    public class BulletsFireConfig : BaseGunShotConfig<Bullet>
    {
        [SerializeField] private float _speed;
        public float Speed => _speed;
    }
}
