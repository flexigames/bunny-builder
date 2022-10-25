using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour
{
    public List<GameObject> phases;

    public int currentPhase = 0;

    public void IncreasePhase()
    {
        phases[currentPhase].SetActive(false);
        if (currentPhase < phases.Count - 1)
        {
            currentPhase++;
            phases[currentPhase].SetActive(true);
        }
    }
}
