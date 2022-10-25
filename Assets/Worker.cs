using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MouseInteractable
{
    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;

    private List<Resource> holding = new List<Resource>();

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
        var successful = agent.SetDestination(destination);
        if (!successful)
        {
            Debug.LogError("Failed to set destination");
        }
    }

    void Update()
    {
        FlipDirection();
        MoveHolding();
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
        StopWorking();
        spriteRenderer.flipY = true;
    }

    void StopWorking()
    {
        if (currentAction != null)
        {
            StopCoroutine(currentAction);
        }
        DropHolding();
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
        for (int index = 0; index < holding.Count; index++)
        {
            var resource = holding[index];
            var offset = (index + 1) * 0.5f / (holding.Count + 1);
            resource.transform.position =
                transform.position + new Vector3(spriteRenderer.flipX ? -offset : offset, 0.5f, 0);
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
        SetHolding(resource.GetComponent<Resource>());
    }

    void SetHolding(Resource resource)
    {
        holding = new List<Resource> { resource };
    }

    void DropHolding()
    {
        holding = new List<Resource>();
    }

    IEnumerator CarryToDropOff()
    {
        var resourcesPile = WorkStation.workStations[WorkStationType.DropOff] as DropOff;

        var destination =
            resourcesPile.transform.position
            + new Vector3(Random.Range(-1f, 0f), Random.Range(-1f, 0f), 0);
        SetDestination(destination);
        yield return new WaitUntil(() => Vector3.Distance(transform.position, destination) < 0.1f);

        foreach (var resource in holding)
        {
            resourcesPile.Add(resource);
        }
        DropHolding();
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

    IEnumerator GoToRandomWorkPlace()
    {
        yield return new WaitForSeconds(0.1f);

        var workPlaces = workStation.workPlaces;
        var workPlace = workPlaces[Random.Range(0, workPlaces.Count)];
        SetDestination(workPlace.transform.position);
        yield return new WaitUntil(
            () => Vector3.Distance(transform.position, workPlace.transform.position) < 0.1f
        );
    }

    IEnumerator ProductionJob()
    {
        while (true)
        {
            yield return GoToRandomWorkPlace();

            yield return Produce();

            yield return CarryToDropOff();
        }
    }

    IEnumerator WaitForEnoughResources()
    {
        var resourcesPile = WorkStation.workStations[WorkStationType.DropOff] as DropOff;

        yield return new WaitUntil(() => resourcesPile.HasEnoughResources());

        holding.Add(resourcesPile.Remove(ResourceType.Wood));
        holding.Add(resourcesPile.Remove(ResourceType.Stone));
    }

    IEnumerator GoToConstructionSite()
    {
        var site = WorkStation.workStations[WorkStationType.Construction];
        SetDestination(site.transform.position);
        yield return new WaitUntil(
            () => Vector3.Distance(transform.position, site.transform.position) < 0.1f
        );
    }

    IEnumerator Construct()
    {
        foreach (var resource in holding)
        {
            Destroy(resource.gameObject);
        }
        DropHolding();
        yield return new WaitForSeconds(2);
    }

    IEnumerator ConstructionJob()
    {
        while (true)
        {
            yield return GoToDropOff();
            yield return WaitForEnoughResources();
            yield return GoToConstructionSite();
            yield return Construct();
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
