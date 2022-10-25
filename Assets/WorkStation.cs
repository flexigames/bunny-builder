using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorkStationType
{
    DropOff,
    Wood
}

public class WorkStation : MouseInteractable
{
    public WorkStationType type;

    public static Dictionary<WorkStationType, WorkStation> workStations =
        new Dictionary<WorkStationType, WorkStation>();

    public override void Awake()
    {
        base.Awake();

        workStations[type] = this;
    }

    override public void OnClickUp()
    {
        if (MouseManager.GrabbedObject is Worker)
        {
            var worker = MouseManager.GrabbedObject as Worker;
            worker.SetWorkStation(this);
        }
    }

    void Update()
    {
        if (!MouseManager.IsGrabbing)
        {
            Unhighlight();
        }
    }

    public override void OnMouseIn()
    {
        Debug.Log("Is grabbing: " + MouseManager.IsGrabbing);
        if (!MouseManager.IsGrabbing)
            return;

        Highlight();
    }

    public override void OnMouseOut()
    {
        Unhighlight();
    }
}
