using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FallingObject : MonoBehaviour
{
    private Rigidbody rb;
    private bool frozen;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        frozen = false;
    }

    public void Unfreeze()
    {
        frozen = false;
        StartCoroutine(FreezeCheck());
    }

    private IEnumerator FreezeCheck()
    {
        yield return new WaitForSeconds(0.1f);

        if (!frozen)
        {
            if (rb.velocity == Vector3.zero)
            {
                Debug.Log("velocity is zero");
                FreezeConstraints();
                frozen = true;
                yield return null;
            }
        }

        StartCoroutine(FreezeCheck());
    }

    public void FreezeConstraints()
    {
        frozen = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.isKinematic = true;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject != transform.parent.gameObject)
    //    {
    //        if (collision.gameObject.CompareTag("Ground"))
    //        {
    //            rb.constraints = RigidbodyConstraints.FreezeAll;
    //            rb.isKinematic = true;
    //        }
    //    }
    //}
}
