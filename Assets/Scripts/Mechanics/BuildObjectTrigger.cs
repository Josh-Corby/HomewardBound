using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildObjectTrigger : GameBehaviour
{
    public List<GameObject> collisionObjects = new List<GameObject>();
    public bool canBuild;

    [SerializeField]
    private ObjectBuild ObjectMain;


    private void Start()
    {
        UpdateCanBuild();
    }
    private void OnEnable()
    {
        collisionObjects.Clear();
        UpdateCanBuild();
    }

    private void OnDisable()
    {
        collisionObjects.Clear();
        UpdateCanBuild();
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

    private void UpdateCanBuild()
    {
        canBuild = collisionObjects.Count == 0;

        if (ObjectMain == null) return;
        ObjectMain.CheckSegmentCollisions(this);
    }


    //public IEnumerator LerpAlpha()
    //{
    //    objectColor = Color.Lerp(transparent, opaque, 2f);
    //    renderer.material.color = objectColor;
    //    yield return new WaitForEndOfFrame();
    //}
}
