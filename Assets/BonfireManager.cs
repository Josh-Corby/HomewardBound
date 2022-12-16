using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BonfireManager : GameBehaviour
{
    [SerializeField]
    private Transform _spawnPoint;
    [SerializeField]
    private GameObject _particles;
    [SerializeField]
    private BoxCollider _col;
    [SerializeField]
    private AudioSource _sounds;
    [SerializeField]
    private AudioClip _bonfireClip;

    private void Awake()
    {
        _col = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == TPM.gameObject)
        {
            _col.enabled = false;
            SetRespawnPoint();
            _sounds.PlayOneShot(_bonfireClip);
        }
    }

    private void SetRespawnPoint()
    {
        GM.SpawnPoint = _spawnPoint;
        _particles.SetActive(true);
    }
}
