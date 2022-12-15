using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    private Rigidbody _rb;
    private bool _frozen;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _frozen = false;
    }

    public void Unfreeze()
    {
        _frozen = false;
        StartCoroutine(FreezeCheck());
    }

    private IEnumerator FreezeCheck()
    {
        yield return new WaitForSeconds(0.1f);
        if (!_frozen)
        {
            if (_rb.velocity == Vector3.zero)
            {
                Debug.Log("velocity is zero");
                FreezeConstraints();
                _frozen = true;
                yield return null;
            }
        }

        StartCoroutine(FreezeCheck());
    }

    public void FreezeConstraints()
    {
        _frozen = true;
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        _rb.isKinematic = true;
    }

}
