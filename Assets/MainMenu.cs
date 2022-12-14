using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public List<Animator> bunnies;

    public float timeBetweenJumps = 2f;

    void Start()
    {
        StartCoroutine(AnimateBunnies());
    }

    IEnumerator AnimateBunnies()
    {
        while (true)
        {
            foreach (var bunny in bunnies)
            {
                bunny.SetTrigger("Jump");
                yield return new WaitForSeconds(timeBetweenJumps);
            }
        }
    }
}
