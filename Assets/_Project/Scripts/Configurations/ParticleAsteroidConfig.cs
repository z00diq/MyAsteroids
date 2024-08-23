using UnityEngine;

namespace Assets.Configurations
{
    [CreateAssetMenu(menuName = "Configs/Enemies/Particle Asteroid", fileName = "Particle Asteroid Config")]
    public class ParticleAsteroidConfig :EnemyConfig
    {
        [SerializeField] private int _minSpawnCount;
        [SerializeField] private int _maxSpawnCount;

        public int SpawnCount => CalculateSpawnCount();

        public int CalculateSpawnCount()
        {
            return Random.Range(_minSpawnCount, _maxSpawnCount+1);
        }
    }
}