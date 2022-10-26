using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Calendar : MonoBehaviour
{
    public TextMeshProUGUI dateText;

    public int year = 1;

    void Start()
    {
        StartCoroutine(RunTime());
    }

    IEnumerator RunTime()
    {
        while (true)
        {
            dateText.text = year + "";
            yield return new WaitForSeconds(2);
            year++;
        }
    }
}
