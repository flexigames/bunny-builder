using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : Singleton<MouseManager>
{
    MouseInteractable mouseTarget;

    MouseInteractable grabbedObject;

    void Update()
    {
        FindMouseTarget();

        MoveGrabbedToMousePosition();

        HandleClicks();
    }

    void FindMouseTarget()
    {
        if (grabbedObject != null)
        {
            return;
        }

        var hits = Physics2D.RaycastAll(
            Camera.main.ScreenToWorldPoint(Input.mousePosition),
            Vector2.zero
        );

        foreach (var hit in hits)
        {
            var mouseInteractable = hit.collider.GetComponent<MouseInteractable>();
            if (mouseInteractable != null)
            {
                mouseTarget = mouseInteractable;
                return;
            }
        }

        if (mouseTarget != null)
        {
            mouseTarget.OnMouseOut();
            mouseTarget = null;
        }
    }

    void MoveGrabbedToMousePosition()
    {
        if (grabbedObject == null)
            return;
        grabbedObject.transform.position = (Vector2)
            Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void HandleClicks()
    {
        if (mouseTarget == null)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (mouseTarget is Grabbable)
            {
                grabbedObject = mouseTarget;
                (mouseTarget as Grabbable).OnGrab();
            }
            else
            {
                mouseTarget.OnClick();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (grabbedObject != null)
            {
                (grabbedObject as Grabbable).OnRelease();
                grabbedObject = null;
            }
            else
            {
                mouseTarget.OnClickUp();
            }
        }
        else
        {
            mouseTarget.OnMouseIn();
        }
    }
}
