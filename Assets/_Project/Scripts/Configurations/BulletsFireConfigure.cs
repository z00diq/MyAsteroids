using Assets.Models;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Ship/Fire/Bullets", fileName = "Ship Bullet Fire Configuration")]
public class BulletsFireConfigure : BaseGunShotConfiguration<Bullet>
{
    [SerializeField] private float _speed;
    public float Speed => _speed;
}
