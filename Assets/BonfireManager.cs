using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BonfireManager : GameBehaviour
{
    [SerializeField]
    private Transform SpawnPoint;
    [SerializeField]
    private GameObject particles;
    [SerializeField]
    private BoxCollider col;
    [SerializeField]
    private AudioSource sounds;
    [SerializeField]
    private AudioClip bonfireClip;

    private void Awake()
    {
        col = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == TPM.gameObject)
        {
            col.enabled = false;
            SetRespawnPoint();
            sounds.PlayOneShot(bonfireClip);
        }
    }

    private void SetRespawnPoint()
    {
        GM.spawnPoint = SpawnPoint;
        particles.SetActive(true);
    }
}
