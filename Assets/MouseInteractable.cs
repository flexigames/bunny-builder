using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MouseInteractable : MonoBehaviour
{
    public virtual void Awake()
    {
        var collider = GetComponent<Collider2D>();
        if (!collider)
        {
            Debug.LogError("MouseInteractable must have a 2D collider");
        }
    }

    public virtual void OnClick() { }

    public virtual void OnClickUp() { }

    public virtual void OnMouseIn() { }

    public virtual void OnMouseOut() { }
}
