using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : GameBehaviour
{
    [SerializeField]
    private AudioClip[] _walkClips;

    [SerializeField]
    private AudioClip[] _runClips;

    [SerializeField]
    private AudioClip[] _jumpClips;

    [SerializeField]
    private AudioClip[] _landClips;

    private void Step()
    {
        SM.StepSFX.PlayOneShot(_walkClips[Random.Range(0, _walkClips.Length)]);
    }

    private void Run()
    {
        SM.StepSFX.PlayOneShot(_walkClips[Random.Range(0, _runClips.Length)]);
    }

    private void Jump()
    {
        SM.StepSFX.PlayOneShot(_walkClips[Random.Range(0, _jumpClips.Length)]);
    }

    private void Land()
    {
        SM.StepSFX.PlayOneShot(_walkClips[Random.Range(0, _landClips.Length)]);
    }
}
