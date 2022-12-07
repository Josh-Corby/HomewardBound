using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : GameBehaviour
{
    public enum RotateDirection { X, Z}
    public RotateDirection direction;
    [SerializeField]
    private Transform FallingObjectSpawnPosition;
    [SerializeField]
    private GameObject Child;

    private FallingObject fallingObject;
    private Rigidbody FallingObjectRB;

    private Outline outline;
    [SerializeField]
    private bool isVisible;


    [SerializeField]
    private string rotateDirection;
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
                FallingObjectRB.constraints &= ~RigidbodyConstraints.FreezeRotationY;
                switch (direction)
                {
                    case RotateDirection.X:
                        FallingObjectRB.constraints &= ~RigidbodyConstraints.FreezeRotationX;
                        break;
                    case RotateDirection.Z:
                        FallingObjectRB.constraints &= ~RigidbodyConstraints.FreezeRotationZ;
                        break;
                }

                

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
