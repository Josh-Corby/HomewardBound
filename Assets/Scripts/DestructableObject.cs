using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : GameBehaviour
{
    public GameObject childObject;
    private Rigidbody childRB;

    private void Start()
    {
        childRB = childObject.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if(childObject != null)
            {
                childRB.useGravity = true;
                childRB.constraints = ~RigidbodyConstraints.FreezePosition;
            }
        
            Destroy(this.gameObject);
        }
    }
}
