using Assets.Models;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Ship/Fire/Laser", fileName = "Ship Laser Fire Configuration")]
public class LaserFireConfigure : BaseGunShotConfiguration<Laser>
{
    [SerializeField] private float _lifeTime;
    [SerializeField] private int _fireMaxCount;

    public float LifeTime => _lifeTime;
    public int FireMaxCount => _fireMaxCount;
}
