using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : SingletonMonoBehaviour<SEManager>
{
    private AudioSource aSource;
    private void Awake()
    {
        aSource = GetComponent<AudioSource>();
    }

    public void PlayOneShot(AudioClip clip, float volume)
    {
        aSource.PlayOneShot(clip, volume);
    }
}
