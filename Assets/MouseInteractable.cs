using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MouseInteractable : MonoBehaviour
{
    protected virtual void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            OnClickUp();
        }
    }

    public virtual void OnClick() { }

    protected virtual void OnClickUp() { }

    public virtual void OnMouseIn() { }

    public virtual void OnMouseOut() { }
}
