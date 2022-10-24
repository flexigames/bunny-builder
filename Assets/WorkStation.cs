using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStation : MouseInteractable
{
    override public void OnClickUp()
    {
        if (MouseManager.GrabbedObject is Worker)
        {
            var worker = MouseManager.GrabbedObject as Worker;
            worker.SetJob("collecting");
        }
    }

    void Update()
    {
        if (!MouseManager.IsGrabbing)
        {
            Unhighlight();
        }
    }

    void Highlight()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.55f, 0.75f, 0.75f);
    }

    void Unhighlight()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
    }

    public override void OnMouseIn()
    {
        if (!MouseManager.IsGrabbing)
            return;

        Highlight();
    }

    public override void OnMouseOut()
    {
        Unhighlight();
    }
}
