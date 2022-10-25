using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip bunnyGiggle;

    public void PlayGiggle()
    {
        var audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(bunnyGiggle);
    }
}
