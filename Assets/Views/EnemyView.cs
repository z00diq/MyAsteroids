using Assets.Models;
using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyView : MonoBehaviour
{
    private BaseEnemy _model;

    public Vector2 ModelSize { get; private set; }

    public event Action<Collider> TriggerDetected;

    public void Initialize(BaseEnemy model)
    {
        _model = model;
        _model.Moved += Moved;
        _model.Died += OnDie;
        _model.EnableGameObject += OnEnable;
        _model.DisableGameObject += OnDisable;
        Renderer renderer = GetComponent<Renderer>();
        ModelSize = new Vector2(renderer.bounds.size.x / 2, renderer.bounds.size.y / 2);
    }

    private void OnEnable()
    {
        if (_model == null)
            return;

        _model.Moved += Moved;
        _model.Died += OnDie;

    }

    private void OnDisable()
    {
        if (_model == null)
            return;

        _model.Moved -= Moved;
        _model.Died -= OnDie;
    }

    private void OnDestroy()
    {
        if (_model == null)
            return;

        _model.EnableGameObject -= OnEnable;
        _model.DisableGameObject -= OnDisable;
        _model.Moved -= Moved;
        _model.Died -= OnDie;
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
        TriggerDetected?.Invoke(other);
    }
}
