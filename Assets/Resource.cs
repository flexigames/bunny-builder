using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Wood,
    Stone
}

public class Resource : MouseInteractable
{
    public ResourceType type;

    void Start()
    {
        isGrabbable = true;
    }

    public void SetOnPile()
    {
        isGrabbable = false;
    }
}
