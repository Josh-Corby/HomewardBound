using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : GameBehaviour<SoundManager>
{
    public AudioSource SFX;
    public AudioSource StepSFX;
    public AudioClip PickupClip;
    public AudioSource WebSFX;

    public void PlaySFXClip(AudioClip clip)
    {
        SFX.clip = clip;
        SFX.Play();
    }
}
