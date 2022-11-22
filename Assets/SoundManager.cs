using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : GameBehaviour<SoundManager>
{
    [SerializeField]
    private AudioSource SFX;
    [SerializeField]
    private AudioSource BGM;
    public AudioClip pickupClip;

    public void PlayClip(AudioClip clip)
    {
        SFX.clip = clip;
        SFX.Play();
    }
}
