using Assets.Models;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyView : BaseView<BaseEnemy>
{
    public override void Initialize(BaseEnemy model)
    {
        base.Initialize(model);
        RenderSize.Moved += Moved;
        RenderSize.Died += OnDie;
        RenderSize.EnableGameObject += OnEnable;
        RenderSize.DisableGameObject += OnDisable;
        
    }

    private void OnEnable()
    {
        if (RenderSize == null)
            return;

        RenderSize.Moved += Moved;
        RenderSize.Died += OnDie;

    }

    private void OnDisable()
    {
        if (RenderSize == null)
            return;

        RenderSize.Moved -= Moved;
        RenderSize.Died -= OnDie;
    }

    private void OnDestroy()
    {
        if (RenderSize == null)
            return;

        RenderSize.EnableGameObject -= OnEnable;
        RenderSize.DisableGameObject -= OnDisable;
        RenderSize.Moved -= Moved;
        RenderSize.Died -= OnDie;
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
        RenderSize.TriggerDetected(other);
    }
}
