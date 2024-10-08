﻿using Assets.Views;
using UnityEngine;

namespace Assets.Configurations
{
    [CreateAssetMenu(menuName ="Configs/Ship",fileName ="Ship Config")]
    public class ShipConfig : ScriptableObject
    {
        [SerializeField] private ShipView _prefab;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _deltaSpeed;
        [SerializeField] private float _rotationSpeed;

        public ShipView Prefab => _prefab;
        public float MaxSpeed => _maxSpeed;
        public float DeltaSpeed => _deltaSpeed;
        public float RotationSpeed => _rotationSpeed;
    }
}
