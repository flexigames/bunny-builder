using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MouseInteractable
{
    void Start() { }

    void Update()
    {
        base.Update();
    }

    public override void OnMouseIn()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1f);
    }

    public override void OnMouseOut()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
