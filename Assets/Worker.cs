using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MouseInteractable, Grabbable
{
    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;

    private string job = "idle";

    public bool isGrabbed = false;

    private Resource holding;

    private WorkStation currentWorkStation;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    void Update()
    {
        FlipDirection();
        MoveHolding();
        if (job == "collecting")
        {
            Collect();
        }
    }

    void FlipDirection()
    {
        if (agent.velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (agent.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
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
        if (isGrabbed)
            return;

        Highlight();
    }

    public override void OnMouseOut()
    {
        Unhighlight();
    }

    public void OnGrab()
    {
        isGrabbed = true;
        Unhighlight();
        spriteRenderer.flipY = true;
    }

    public void OnRelease()
    {
        spriteRenderer.flipY = false;
        Unhighlight();
        agent.SetDestination(transform.position);
        isGrabbed = false;
    }

    public void SetJob(string job)
    {
        this.job = job;
    }

    public void SetWorkStation(WorkStation workStation)
    {
        currentWorkStation = workStation;
    }

    public void MoveHolding()
    {
        if (holding != null)
        {
            holding.transform.position = transform.position;
        }
    }

    public void Collect()
    {
        if (holding == null)
        {
            var resource = FindClosestResource();
            if (resource == null)
            {
                SetJob("idle");
                SetWorkStation(null);
                return;
            }

            if (Vector3.Distance(transform.position, resource.transform.position) < 0.5f)
            {
                holding = resource;
            }
            else
            {
                SetDestination(resource.transform.position);
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, currentWorkStation.transform.position) < 0.5f)
            {
                holding = null;
                SetJob("idle");
                SetWorkStation(null);
            }
            else
            {
                SetDestination(currentWorkStation.transform.position);
            }
        }
    }

    Resource FindClosestResource()
    {
        Resource closestResource = null;
        float closestDistance = 0;
        var resources = FindObjectsOfType<Resource>();
        foreach (var resource in resources)
        {
            var distance = Vector3.Distance(transform.position, resource.transform.position);
            if (closestResource == null || distance < closestDistance)
            {
                closestResource = resource;
                closestDistance = distance;
            }
        }
        return closestResource;
    }
}
