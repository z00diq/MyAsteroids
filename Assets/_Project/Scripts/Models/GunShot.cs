namespace Assets.Models
{
    public interface IGunShot
    {
        public DamageType DamageType { get; }
    }

    public enum DamageType
    {
        Bullet,
        Laser
    }
}