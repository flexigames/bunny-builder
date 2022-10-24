using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MouseInteractable, Grabbable
{
    void Start() { }

    void Update() { }

    public override void OnMouseIn()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1f);
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
