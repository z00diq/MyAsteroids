using UnityEngine;

public class BaseView<T> : MonoBehaviour where T : class
{
    public Vector2 Size { get; private set; }
    public T Model { get; private set; }

    private void Awake()
    {
        if (TryGetComponent(out Renderer renderer))
            Size = new Vector2(renderer.bounds.size.x / 2, renderer.bounds.size.y / 2);
    }

    public virtual void Initialize(T model)
    {
        Model = model;
    }
}
