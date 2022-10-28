using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : GameBehaviour
{
    [SerializeField]
    private Transform FallingObjectSpawnPosition;
    [SerializeField]
    private GameObject FallingObject;
    private Rigidbody FallingObjectRB;

    private bool isDestroyed;

    private void Start()
    {
        FallingObjectSpawnPosition.position = FallingObject.transform.position;
        FallingObjectRB = FallingObject.GetComponent<Rigidbody>();
        GameManager.OnPlayerRespawn += ResetObject;
        ResetObject();
    }

    private void ResetObject()
    {
        gameObject.SetActive(true);
        FallingObject.transform.position = FallingObjectSpawnPosition.position;
        FallingObjectRB.useGravity = false;
        FallingObjectRB.constraints = RigidbodyConstraints.FreezeAll;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Bullet hit");
            if(FallingObject != null)
            {

                FallingObjectRB.useGravity = true;
                FallingObjectRB.constraints &= ~RigidbodyConstraints.FreezePositionY;


            }

            
        }
    }
}
