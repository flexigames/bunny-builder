using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOff : WorkStation
{
    public Dictionary<ResourceType, List<Resource>> resources =
        new Dictionary<ResourceType, List<Resource>>();

    void Start()
    {
        resources[ResourceType.Wood] = new List<Resource>();
        resources[ResourceType.Stone] = new List<Resource>();
    }

    public void Add(Resource resource)
    {
        resources[resource.type].Add(resource);
    }

    public Resource Remove(ResourceType resourceType)
    {
        if (resources[resourceType].Count == 0)
            return null;

        var resource = resources[resourceType][0];
        resources[resourceType].Remove(resource);
        return resource;
    }

    public bool HasEnoughResources()
    {
        return resources[ResourceType.Wood].Count >= 1 && resources[ResourceType.Stone].Count >= 1;
    }
}
