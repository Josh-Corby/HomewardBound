using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : GameBehaviour
{
    public enum RotateDirection { X, Z}
    public RotateDirection Direction;
    [SerializeField]
    private Transform _fallingObjectSpawnPosition;
    [SerializeField]
    private GameObject _child;

    private FallingObject _fallingObject;

    private Rigidbody _fallingObjectRB;

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
        _fallingObjectSpawnPosition.position = _child.transform.position;
        _fallingObjectRB = _child.GetComponent<Rigidbody>();
        _fallingObject = _child.GetComponent<FallingObject>();
    }

    private void ResetObject()
    {
        gameObject.SetActive(true);
        _child.transform.position = _fallingObjectSpawnPosition.position;
        _fallingObjectRB.useGravity = false;
        _fallingObjectRB.constraints = RigidbodyConstraints.FreezeAll;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Bullet hit");
            if (_fallingObjectRB != null)
            {
                _fallingObjectRB.useGravity = true;
                _fallingObjectRB.constraints &= ~RigidbodyConstraints.FreezePositionY;
                switch (Direction)
                {
                    case RotateDirection.X:
                        _fallingObjectRB.constraints &= ~RigidbodyConstraints.FreezeRotationX;
                        break;
                    case RotateDirection.Z:
                        _fallingObjectRB.constraints &= ~RigidbodyConstraints.FreezeRotationZ;
                        break;
                }
                _fallingObject.Unfreeze();
            }
        }
    }
}
