using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildObjectTrigger : GameBehaviour
{
    public List<GameObject> collisionObjects = new List<GameObject>();
    public bool isNotColliding;

    public ObjectBuild ObjectMain;

    [SerializeField]
    private BoxCollider collider;


    private void OnEnable()
    {
        collisionObjects.Clear();
        UpdateCanBuild();
        collider.isTrigger = true;
    }

    private void UpdateCanBuild()
    {
        isNotColliding = collisionObjects.Count == 0;
        ObjectMain.CanObjectBeBuilt(isNotColliding);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hawk") || other.CompareTag("Mechanics") || other.CompareTag("Player"))
        {
            return;
        }
        collisionObjects.Add(other.gameObject);
        UpdateCanBuild();
    }

    private void OnTriggerExit(Collider other)
    {
        collisionObjects.Remove(other.gameObject);
        UpdateCanBuild();
    }

    


    //public IEnumerator LerpAlpha()
    //{
    //    objectColor = Color.Lerp(transparent, opaque, 2f);
    //    renderer.material.color = objectColor;
    //    yield return new WaitForEndOfFrame();
    //}
}
