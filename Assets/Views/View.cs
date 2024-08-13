using System;
using UnityEngine;

public class View : MonoBehaviour
{
    public Vector2 ModelSize { get; private set; }

    public event Action<Collider> TriggerDetected;

    public void Initialize()
    {
        Renderer renderer = GetComponent<Renderer>();
        ModelSize = new Vector2(renderer.bounds.size.x / 2, renderer.bounds.size.y / 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerDetected?.Invoke(other);
    }
}
