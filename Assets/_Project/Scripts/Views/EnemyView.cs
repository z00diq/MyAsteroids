using Assets.Models;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyView : BaseView<BaseEnemy>
{
    public override void Initialize(BaseEnemy model)
    {
        base.Initialize(model);
        Model.Moved += Moved;
        Model.Died += OnDie;
        Model.EnableGameObject += OnEnable;
        Model.DisableGameObject += OnDisable;
        
    }

    private void OnEnable()
    {
        if (Model == null)
            return;

        Model.Moved += Moved;
        Model.Died += OnDie;

    }

    private void OnDisable()
    {
        if (Model == null)
            return;

        Model.Moved -= Moved;
        Model.Died -= OnDie;
    }

    private void OnDestroy()
    {
        if (Model == null)
            return;

        Model.EnableGameObject -= OnEnable;
        Model.DisableGameObject -= OnDisable;
        Model.Moved -= Moved;
        Model.Died -= OnDie;
    }

    private void OnDie()
    {
        Destroy(gameObject);
    }

    private void Moved(Vector3 obj)
    {
        transform.position = obj;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IGunShot gunShot))
            Model.TriggerDetected(gunShot.DamageType);
    }
}
