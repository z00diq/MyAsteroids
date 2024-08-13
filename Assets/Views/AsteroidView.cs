using Assets.Models;
using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class AsteroidView : View
{
    private BaseEnemy _model;

    public void Initialize(BaseEnemy asteroid)
    {
        base.Initialize();
        _model = asteroid;
        _model.Moved += Moved;
        _model.Died += OnDie;
    }

    private void OnDie()
    {
        Destroy(gameObject);
    }

    private void Moved(Vector3 obj)
    {
        transform.position = obj;
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
        _model.Moved -= Moved;
        _model.Died -= OnDie;
    }
}
