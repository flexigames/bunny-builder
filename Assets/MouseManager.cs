using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : Singleton<MouseManager>
{
    MouseInteractable mouseTarget;

    void FindMouseTarget()
    {
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

    void Update()
    {
        FindMouseTarget();

        if (mouseTarget != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseTarget.OnClick();
            }
            else
            {
                mouseTarget.OnMouseIn();
            }
        }
    }
}
