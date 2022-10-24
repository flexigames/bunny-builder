using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MouseInteractable, Grabbable
{
    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;

    public bool isGrabbed = false;

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
}
