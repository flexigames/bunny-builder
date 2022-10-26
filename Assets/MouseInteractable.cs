using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MouseInteractable : MonoBehaviour
{
    public bool isGrabbable = false;
    public bool isGrabbed = false;

    public bool isActive = true;

    private Color originalColor;

    public virtual void Awake()
    {
        var collider = GetComponent<Collider2D>();
        if (!collider)
        {
            Debug.LogError("MouseInteractable must have a 2D collider");
        }

        originalColor = GetComponent<SpriteRenderer>().color;
    }

    public virtual void OnClick() { }

    public virtual void OnClickUp() { }

    public virtual void OnMouseIn()
    {
        if (!isGrabbable || isGrabbed)
            return;

        Highlight();
    }

    public virtual void OnMouseOut()
    {
        Unhighlight();
    }

    public virtual void OnGrab()
    {
        isGrabbed = true;
        Unhighlight();
    }

    public virtual void OnRelease()
    {
        isGrabbed = false;
    }

    public void Highlight()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.55f, 0.75f, 0.75f);
    }

    public void Unhighlight()
    {
        gameObject.GetComponent<SpriteRenderer>().color = originalColor;
    }
}
