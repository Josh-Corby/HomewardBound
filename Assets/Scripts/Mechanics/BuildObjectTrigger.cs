using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildObjectTrigger : GameBehaviour
{
    public static event Action OnBridgeCollision;

    public List<GameObject> collisionObjects = new List<GameObject>();

    public bool canBuild;

    private GameObject buildObject;
    //MeshRenderer renderer;
    //[SerializeField]
    //Color transparent;
    //[SerializeField]
    //Color opaque;
    //Color objectColor;


    private void OnEnable()
    {
        foreach (GameObject collision in collisionObjects)
        {
            collisionObjects.Remove(collision);
        }
    }

    private void OnDisable()
    {
        foreach (GameObject collision in collisionObjects)
        {
            collisionObjects.Remove(collision);
        }
    }
    private void Awake()
    {
        buildObject = transform.parent.gameObject;

    }
    private void Update()
    {
        canBuild = collisionObjects.Count == 0;
    }



    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Hawk") || other.CompareTag("Mechanics") || other.CompareTag("Player"))
        {
            return;
        }
        //Debug.Log(other.gameObject.name);

        collisionObjects.Add(other.gameObject);

        OnBridgeCollision();
    }

    private void OnTriggerExit(Collider other)
    {
        collisionObjects.Remove(other.gameObject);

        OnBridgeCollision();
    }

    //public IEnumerator LerpAlpha()
    //{
    //    objectColor = Color.Lerp(transparent, opaque, 2f);
    //    renderer.material.color = objectColor;
    //    yield return new WaitForEndOfFrame();
    //}
}
