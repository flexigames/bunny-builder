using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropOff : WorkStation
{
    public Dictionary<ResourceType, List<Resource>> resources =
        new Dictionary<ResourceType, List<Resource>>();

    public List<GameObject> woodLocations = new List<GameObject>();
    public List<GameObject> stoneLocations = new List<GameObject>();

    public TextMeshProUGUI woodText;
    public TextMeshProUGUI stoneText;

    void Start()
    {
        resources[ResourceType.Wood] = new List<Resource>();
        resources[ResourceType.Stone] = new List<Resource>();
        UpdateText();
    }

    void UpdateText()
    {
        woodText.text = resources[ResourceType.Wood].Count.ToString();
        stoneText.text = resources[ResourceType.Stone].Count.ToString();
    }

    public void Add(Resource resource)
    {
        resources[resource.type].Add(resource);
        resource.transform.position = GetLocation(resource.type);
        UpdateText();
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
        UpdateText();
        return resource;
    }

    public bool HasEnoughResources()
    {
        return resources[ResourceType.Wood].Count >= 1 && resources[ResourceType.Stone].Count >= 1;
    }

    public bool HasEnoughResources(int wood, int stone)
    {
        return resources[ResourceType.Wood].Count >= wood
            && resources[ResourceType.Stone].Count >= stone;
    }

    public void RemoveResources(int wood, int stone)
    {
        for (int i = 0; i < wood; i++)
        {
            Remove(ResourceType.Wood);
        }
        for (int i = 0; i < stone; i++)
        {
            Remove(ResourceType.Stone);
        }
    }
}
