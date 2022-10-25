using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MouseInteractable
{
    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;

    private Resource holding;

    public WorkStation workStation;

    void Start()
    {
        isGrabbable = true;

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

        if (workStation?.type == WorkStationType.DropOff)
        {
            DropOffJob();
        }
        else if (workStation?.type == WorkStationType.Wood)
        {
            WoodJob();
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

    override public void OnGrab()
    {
        base.OnGrab();

        spriteRenderer.flipY = true;
        holding = null;
        workStation = null;
    }

    override public void OnRelease()
    {
        base.OnRelease();
        spriteRenderer.flipY = false;
        agent.SetDestination(transform.position);
    }

    public void SetWorkStation(WorkStation newWorkStation)
    {
        workStation = newWorkStation;
    }

    public void MoveHolding()
    {
        if (holding != null)
        {
            holding.transform.position = transform.position;
        }
    }

    public void WoodJob()
    {
        // produce wood for 2 seconds



        // carry wood to pile
        // return to work station
    }

    public void DropOffJob()
    {
        if (holding == null)
        {
            var resource = FindClosestResource();
            if (resource == null)
            {
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
            if (Vector3.Distance(transform.position, workStation.transform.position) < 0.5f)
            {
                holding = null;

                SetWorkStation(null);
            }
            else
            {
                SetDestination(workStation.transform.position);
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
