using Assets.Models;
using UnityEngine;

namespace Assets.Configurations
{
    [CreateAssetMenu(menuName = "Configs/Ship/Fire/Laser", fileName = "Ship Laser Fire Configuration")]
    public class LaserFireConfig : BaseGunShotConfig<Laser>
    {
        [SerializeField] private float _lifeTime;
        [SerializeField] private int _fireMaxCount;

        public float LifeTime => _lifeTime;
        public int FireMaxCount => _fireMaxCount;
    }
}
