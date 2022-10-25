using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public GameObject workerPrefab;
    public Transform workerSpawnPosition;

    public DropOff dropOff;

    void Start()
    {
        dropOff = FindObjectOfType<DropOff>();
    }

    public void Update()
    {
        Button button = GetComponent<Button>();
        button.interactable = CanUpgrade();
    }

    bool CanUpgrade()
    {
        return dropOff.HasEnoughResources(5, 5);
    }

    public void OnClick()
    {
        dropOff.ConsumeResources(5, 5);
        var worker = Instantiate(workerPrefab) as GameObject;
        worker.transform.position =
            workerSpawnPosition.position
            + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
    }
}
