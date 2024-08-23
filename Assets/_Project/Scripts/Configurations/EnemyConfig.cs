using Assets.Views;
using UnityEngine;

namespace Assets.Configurations
{
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField] EnemyView _prefab;
        [SerializeField] int _maxCount;
        [SerializeField] float _minSpeed;
        [SerializeField] float _maxSpeed;
        [SerializeField] float _occurrenceFrequency;
        [SerializeField] float _outBoundsDepth;

        public EnemyView Prefab => _prefab;
        public int MaxCount => _maxCount;
        public float MinSpeed => _minSpeed;
        public float MaxSpeed => _maxSpeed;
        public float OccurrenceFrequency=> _occurrenceFrequency;
        public float OutBoundsDepth => _outBoundsDepth; 
    }
}
