using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogModifyer : GameBehaviour
{
    [SerializeField]
    private bool _turnOffFog;
    public GameObject TreeObject;

    private void Start()
    {
        _turnOffFog = false;
        TreeObject.SetActive(true);
    }
    private void Update()
    {
        if (_turnOffFog)
        {
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, 0.0002f, 0.01f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == TPM.gameObject)
        {
            _turnOffFog = true;
            TreeObject.SetActive(true);
        }
    }
}
