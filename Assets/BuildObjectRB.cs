using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ObjectType
{
    Bridge,
    Ladder
}

public class BuildObjectRB : MonoBehaviour
{
    [SerializeField]
    private ObjectType _objectType;
    public bool Frozen;
    [SerializeField]
    private Rigidbody _rb;

    private void Awake()
    {

        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void Start()
    {
        Frozen = false;
        StartCoroutine(FreezeCheck());
    }

    private IEnumerator FreezeCheck()
    {
        yield return new WaitForSeconds(1f);

        if (!Frozen)
        {
            if (_rb.velocity == Vector3.zero)
            {
                FreezeConstraints();
                Frozen = true;
                yield return null;
            }
        }

        StartCoroutine(FreezeCheck());
    }

    public void FreezeConstraints()
    {
        Frozen = true;
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void UnFreezeConstraints()
    {
        if (_objectType == ObjectType.Bridge)
        {
            //Debug.Log("Unfreeze constraints");
            _rb.constraints = RigidbodyConstraints.None;

        }

        if (_objectType == ObjectType.Ladder)
        {
            _rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == gameObject || collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player") 
            || collision.gameObject.CompareTag("Bridge") || collision.gameObject.CompareTag("Ladder") || collision.gameObject.CompareTag("Bonfire")) return;

        Debug.Log(collision.gameObject.name);
        FreezeConstraints();
    }
}
