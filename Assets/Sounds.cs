using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip bunnyGiggle;

    public void PlayGiggle()
    {
        if (bunnyGiggle == null)
            return;

        var audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(bunnyGiggle);
    }
}
