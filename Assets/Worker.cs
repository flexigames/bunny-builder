using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MouseInteractable, Grabbable
{
    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;

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

    public override void OnMouseIn()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.55f, 0.75f, 0.75f);
    }

    public override void OnMouseOut()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void OnGrab()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void OnRelease()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
