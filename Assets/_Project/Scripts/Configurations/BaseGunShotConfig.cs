using Assets.Views;
using UnityEngine;

namespace Assets.Configurations
{
    public abstract class BaseGunShotConfig<T> : ScriptableObject where T: class
    {
        [SerializeField] private BaseView<T> _prefab;
        [SerializeField] private float _reloadTime;

        public BaseView<T> Prefab => _prefab;
        public float ReloadTime => _reloadTime;
    }
}
