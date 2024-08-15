using UnityEngine;

public class BaseView<T> : MonoBehaviour where T : class
{
    public Vector2 ModelSize { get; private set; }
    public T RenderSize { get; private set; }

    public virtual void Initialize(T model)
    {
        if(TryGetComponent(out Renderer renderer))
            ModelSize = new Vector2(renderer.bounds.size.x / 2, renderer.bounds.size.y / 2);

        RenderSize = model;
    }
}
