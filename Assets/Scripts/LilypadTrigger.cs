using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilypadTrigger : MonoBehaviour
{
    private FallingLilyPad lilypad;

    private void Awake()
    {
        lilypad = GetComponentInParent<FallingLilyPad>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            lilypad.StartFalling();

        }
    }
}
