using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Calendar : MonoBehaviour
{
    public TextMeshProUGUI dateText;

    static public int year = 1;

    public bool isGameCalendar = true;

    public bool isRunning = true;

    void Start()
    {
        if (isGameCalendar)
        {
            Calendar.year = 1;
            StartCoroutine(RunTime());
        }
        else
        {
            UpdateText();
        }
    }

    void UpdateText()
    {
        dateText.text = year + "";
    }

    IEnumerator RunTime()
    {
        while (true)
        {
            UpdateText();
            yield return new WaitForSeconds(2);
            if (!isRunning)
            {
                break;
            }
            Calendar.year++;
        }
    }
}
