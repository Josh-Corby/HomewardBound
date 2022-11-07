using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : GameBehaviour
{
    [SerializeField]
    private Transform FallingObjectSpawnPosition;
    [SerializeField]
    private GameObject Child;

    private FallingObject fallingObject;
    private Rigidbody FallingObjectRB;


    private void Awake()
    {
        FallingObjectSpawnPosition.position = Child.transform.position;
        FallingObjectRB = Child.GetComponent<Rigidbody>();
        fallingObject = Child.GetComponent<FallingObject>();
    }


    private void OnEnable()
    {
        GameManager.OnPlayerRespawn += ResetObject;
    }


    private void OnDisable()
    {
        GameManager.OnPlayerRespawn -= ResetObject;
    }
    private void ResetObject()
    {
        gameObject.SetActive(true);
        Child.transform.position = FallingObjectSpawnPosition.position;
        FallingObjectRB.useGravity = false;
        FallingObjectRB.constraints = RigidbodyConstraints.FreezeAll;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Bullet hit");
            if (FallingObjectRB != null)
            {
                FallingObjectRB.useGravity = true;
                FallingObjectRB.constraints &= ~RigidbodyConstraints.FreezePositionY;
                FallingObjectRB.constraints &= ~RigidbodyConstraints.FreezeRotationZ;
                fallingObject.Unfreeze();
            }
        }
    }
}
