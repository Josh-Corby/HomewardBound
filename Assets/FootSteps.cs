using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : GameBehaviour
{

    [SerializeField]
    private AudioClip[] walkClips;

    [SerializeField]
    private AudioClip[] runClips;

    [SerializeField]
    private AudioClip[] jumpClips;

    [SerializeField]
    private AudioClip[] landClips;
    private void Step()
    {
        SM.StepSFX.PlayOneShot(walkClips[Random.Range(0, walkClips.Length)]);
    }

    private void Run()
    {
        SM.StepSFX.PlayOneShot(walkClips[Random.Range(0, runClips.Length)]);
    }

    private void Jump()
    {
        SM.StepSFX.PlayOneShot(walkClips[Random.Range(0, jumpClips.Length)]);
    }

    private void Land()
    {
        SM.StepSFX.PlayOneShot(walkClips[Random.Range(0, landClips.Length)]);
    }
}
