using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour
{
    public List<GameObject> phases;

    public int currentPhase = 0;

    public int buildStepsPerPhase = 2;

    public int buildSteps = 0;

    int totalBuildSteps;

    public GameObject statueBar;

    void Start()
    {
        totalBuildSteps = (phases.Count - 1) * buildStepsPerPhase;
    }

    public bool IsDone()
    {
        return currentPhase == phases.Count - 1;
    }

    public void Build()
    {
        if (currentPhase >= phases.Count - 1)
            return;

        buildSteps++;
        if (buildSteps % buildStepsPerPhase == 0)
        {
            IncreasePhase();
        }

        UpdateBar();
    }

    void UpdateBar()
    {
        statueBar.transform.localScale = new Vector3(1, (float)buildSteps / totalBuildSteps, 1);
    }

    void IncreasePhase()
    {
        phases[currentPhase].SetActive(false);
        if (currentPhase < phases.Count - 1)
        {
            currentPhase++;
            phases[currentPhase].SetActive(true);
        }
    }
}
