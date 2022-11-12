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

    private Outline outline;
    [SerializeField]
    private bool isVisible;


    private void OnEnable()
    {
        GameManager.OnPlayerRespawn += ResetObject;
    }
    private void OnDisable()
    {
        GameManager.OnPlayerRespawn -= ResetObject;
    }
    private void Awake()
    {
        FallingObjectSpawnPosition.position = Child.transform.position;
        FallingObjectRB = Child.GetComponent<Rigidbody>();
        fallingObject = Child.GetComponent<FallingObject>();
        outline = GetComponent<Outline>();
    }
    private void Update()
    {
        //if (isVisible)
        //{
        //    if (Vector3.Distance(gameObject.transform.position, TPM.gameObject.transform.position) <= 50)
        //    {
        //        outline.enabled = true;
        //        return;
        //    }
        //    else
        //    {
        //        outline.enabled = false;
        //        return;
        //    }
        //}
        //if (!isVisible)
        //{
        //    outline.enabled = false;
        //    return;
        //}
        
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

    private void OnBecameVisible()
    {

        isVisible = true;
    }


    private void OnBecameInvisible() 
    {
        isVisible = false;
    }
}
