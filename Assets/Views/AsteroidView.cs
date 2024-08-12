using Assets.Models;
using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class AsteroidView : View
{
    private Asteroid _model;

    public event Action<Collider> TriggerDetected;

    public void Initialize(Asteroid asteroid)
    {

        base.Initialize();
        _model = asteroid;
        _model.Moved += Moved;
        
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
    }

    private void OnDisable()
    {
        _model.Moved -= Moved;
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerDetected?.Invoke(other);      
    }
}

public class View : MonoBehaviour
{
    public Vector2 ModelSize { get; private set; }

    protected void Initialize()
    {
        Renderer renderer = GetComponent<Renderer>();
        ModelSize = new Vector2(renderer.bounds.size.x / 2, renderer.bounds.size.y / 2);
    }
}
