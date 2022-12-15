using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildObjectTrigger : GameBehaviour
{
    public List<GameObject> CollisionObjects = new List<GameObject>();
    public bool IsNotColliding;
    public ObjectBuild ObjectMain;
    [SerializeField]
    private BoxCollider _collider;

    private void OnEnable()
    {
        CollisionObjects.Clear();
        UpdateCanBuild();
        _collider.isTrigger = true;
    }
    private void UpdateCanBuild()
    {
        IsNotColliding = CollisionObjects.Count == 0;
        ObjectMain.CanObjectBeBuilt(IsNotColliding);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mechanics") || other.CompareTag("Player") || other.CompareTag("Bonfire") || other.gameObject == IZ.gameObject)
        {
            return;
        }
        CollisionObjects.Add(other.gameObject);
        UpdateCanBuild();
    }

    private void OnTriggerExit(Collider other)
    {
        CollisionObjects.Remove(other.gameObject);
        UpdateCanBuild();
    }

   
}
