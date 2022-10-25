using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum WorkStationType
{
    DropOff,
    Production,
    Construction
}

public class WorkStation : MouseInteractable
{
    public WorkStationType type;

    public List<GameObject> workPlaces = new List<GameObject>();

    public GameObject resourcePrefab;

    public TextMeshProUGUI workerText;

    public static Dictionary<WorkStationType, WorkStation> workStations =
        new Dictionary<WorkStationType, WorkStation>();

    public override void Awake()
    {
        base.Awake();

        workStations[type] = this;
    }

    void UpdateText()
    {
        var workers = CountNumberOfWorkers();
        if (workerText != null)
            workerText.text = workers.ToString();
    }

    int CountNumberOfWorkers()
    {
        var workers = FindObjectsOfType<Worker>();
        var count = 0;
        foreach (var worker in workers)
        {
            if (worker.workStation == this)
            {
                count++;
            }
        }
        return count;
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
        UpdateText();
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
