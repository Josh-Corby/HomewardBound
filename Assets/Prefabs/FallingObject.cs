using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject != transform.parent.gameObject)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
            rb.constraints = RigidbodyConstraints.FreezeAll;
                rb.isKinematic = true;
            }
        }
    }
}
