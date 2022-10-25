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

    private IEnumerator currentAction;

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
        if (workStation?.type == WorkStationType.Production)
        {
            StartProductionJob();
        }
        else if (workStation?.type == WorkStationType.Construction)
        {
            StartConstructionJob();
        }
    }

    public void MoveHolding()
    {
        if (holding != null)
        {
            holding.transform.position =
                transform.position + new Vector3(spriteRenderer.flipX ? -0.5f : 0.5f, 0.5f, 0);
        }
    }

    public void StartProductionJob()
    {
        currentAction = ProductionJob();
        StartCoroutine(currentAction);
    }

    public void StartConstructionJob()
    {
        currentAction = ConstructionJob();
        StartCoroutine(currentAction);
    }

    IEnumerator Produce()
    {
        yield return new WaitForSeconds(2);
        var resourcePrefab = workStation.resourcePrefab;
        var resource = Instantiate(resourcePrefab) as GameObject;
        resource.transform.position = transform.position;
        holding = resource.GetComponent<Resource>();
    }

    IEnumerator CarryToDropOff()
    {
        var resourcesPile = WorkStation.workStations[WorkStationType.DropOff] as DropOff;

        var destination =
            resourcesPile.transform.position
            + new Vector3(Random.Range(-1f, 0f), Random.Range(-1f, 0f), 0);
        SetDestination(destination);
        yield return new WaitUntil(() => Vector3.Distance(transform.position, destination) < 0.1f);

        resourcesPile.Add(holding);
        holding = null;
    }

    IEnumerator GoToDropOff()
    {
        yield return new WaitForSeconds(0.5f);
        var resourcesPile = WorkStation.workStations[WorkStationType.DropOff];
        SetDestination(resourcesPile.transform.position);
        yield return new WaitUntil(
            () => Vector3.Distance(transform.position, resourcesPile.transform.position) < 0.1f
        );
    }

    IEnumerator GoToWorkStation()
    {
        SetDestination(workStation.transform.position);
        yield return new WaitUntil(
            () => Vector3.Distance(transform.position, workStation.transform.position) < 0.1f
        );
    }

    IEnumerator ProductionJob()
    {
        while (true)
        {
            yield return Produce();

            yield return CarryToDropOff();

            yield return GoToWorkStation();
        }
    }

    IEnumerator WaitForEnoughResources()
    {
        var resourcesPile = WorkStation.workStations[WorkStationType.DropOff] as DropOff;

        yield return new WaitUntil(() => resourcesPile.HasEnoughResources());

        holding = resourcesPile.Remove(ResourceType.Wood);
    }

    IEnumerator GoToConstructionSite()
    {
        var site = WorkStation.workStations[WorkStationType.Construction];
        SetDestination(site.transform.position);
        yield return new WaitUntil(
            () => Vector3.Distance(transform.position, site.transform.position) < 0.1f
        );
        holding = null;
    }

    IEnumerator ConstructionJob()
    {
        while (true)
        {
            yield return GoToDropOff();
            yield return WaitForEnoughResources();
            yield return GoToConstructionSite();
        }
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
