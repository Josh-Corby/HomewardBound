using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableMaterial : GameBehaviour
{
    [SerializeField]
    private bool _isMovingTowardsPlayer;
    private Collider _col;

    private readonly float _moveSpeed = 0.3f;

    private void Awake()
    {
        _isMovingTowardsPlayer = false;
        _col = gameObject.GetComponent<Collider>();
    }

    private void Update()
    {

        if (_isMovingTowardsPlayer)
        {
            MoveTowardsPlayer();
        }

        if (Vector3.Distance(transform.position, PM.pickUpSpot.transform.position) <= 0.1f)
        {
            GM.IncreaseResources(gameObject);
            if (SM.SFX != null)
            {
                SM.PlaySFXClip(SM.PickupClip);
            }

            gameObject.SetActive(false);
        }
    }
    public void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, PM.pickUpSpot.transform.position, _moveSpeed);
    }

    public void StartMovingTowardsPlayer()
    {
        _isMovingTowardsPlayer = true;
        _col.isTrigger = true;
    }
}
