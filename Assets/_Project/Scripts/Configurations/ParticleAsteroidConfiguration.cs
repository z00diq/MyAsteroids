using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Enemies/Particle Asteroid", fileName = "Particle Asteroid Config")]
public class ParticleAsteroidConfiguration :EnemyConfiguration
{
    [SerializeField] private int _minSpawnCount;
    [SerializeField] private int _maxSpawnCount;

    public int SpawnCount => CalculateSpawnCount();

    public int CalculateSpawnCount()
    {
        return Random.Range(_minSpawnCount, _maxSpawnCount+1);
    }
}