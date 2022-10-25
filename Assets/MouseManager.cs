using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : Singleton<MouseManager>
{
    public MouseInteractable mouseTarget;

    public MouseInteractable grabbedObject;

    public static bool IsGrabbing
    {
        get { return _instance?.grabbedObject != null; }
    }

    public static MouseInteractable GrabbedObject
    {
        get { return _instance?.grabbedObject; }
    }

    void Update()
    {
        FindMouseTarget();

        MoveGrabbedToMousePosition();

        HandleClicks();
    }

    void FindMouseTarget()
    {
        MouseInteractable newTarget = null;

        var hits = Physics2D.RaycastAll(
            Camera.main.ScreenToWorldPoint(Input.mousePosition),
            Vector2.zero
        );

        foreach (var hit in hits)
        {
            var mouseInteractable = hit.collider.GetComponent<MouseInteractable>();
            if (mouseInteractable != null && mouseInteractable != grabbedObject)
            {
                newTarget = mouseInteractable;
                newTarget?.OnMouseIn();
                break;
            }
        }

        if (mouseTarget != newTarget)
        {
            mouseTarget?.OnMouseOut();
        }

        mouseTarget = newTarget;
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
        if (Input.GetMouseButtonDown(0))
        {
            if (mouseTarget != null)
            {
                if (mouseTarget.isGrabbable)
                {
                    grabbedObject = mouseTarget;
                    mouseTarget.OnGrab();
                }
                else
                {
                    mouseTarget.OnClick();
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mouseTarget?.OnClickUp();

            if (grabbedObject != null)
            {
                grabbedObject.OnRelease();
                grabbedObject = null;
            }
        }
    }
}
