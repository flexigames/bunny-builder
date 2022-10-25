using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOff : WorkStation
{
    public Dictionary<ResourceType, List<Resource>> resources =
        new Dictionary<ResourceType, List<Resource>>();

    public List<GameObject> woodLocations = new List<GameObject>();
    public List<GameObject> stoneLocations = new List<GameObject>();

    void Start()
    {
        resources[ResourceType.Wood] = new List<Resource>();
        resources[ResourceType.Stone] = new List<Resource>();
    }

    public void Add(Resource resource)
    {
        resources[resource.type].Add(resource);
        resource.transform.position = GetLocation(resource.type);
    }

    Vector3 GetLocation(ResourceType type)
    {
        if (type == ResourceType.Wood)
        {
            var index = Mathf.Min(resources[ResourceType.Wood].Count - 1, woodLocations.Count - 1);
            var location = woodLocations[index];
            return location.transform.position;
        }
        else
        {
            var index = Mathf.Min(
                resources[ResourceType.Stone].Count - 1,
                stoneLocations.Count - 1
            );
            var location = stoneLocations[index];
            return location.transform.position;
        }
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
