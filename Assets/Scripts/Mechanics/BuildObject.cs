using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObject : GameBehaviour
{
    public List<GameObject> collisionObjects = new List<GameObject>();

    GameObject buildObject;
    //MeshRenderer renderer;
    //[SerializeField]
    //Color transparent;
    //[SerializeField]
    //Color opaque;
    //Color objectColor;


    private void Awake()
    {
        buildObject = transform.parent.gameObject;
        //renderer = buildObject.GetComponent<MeshRenderer>();
        //objectColor = renderer.material.color;
    }
    private void Update()
    {

        BM.canBuild = collisionObjects.Count == 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Hawk") || other.CompareTag("Mechanics") || other.CompareTag("Player"))
        {
            return;
        }

        Debug.Log(other.gameObject.name);

        collisionObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        collisionObjects.Remove(other.gameObject);
    }

    //public IEnumerator LerpAlpha()
    //{
    //    objectColor = Color.Lerp(transparent, opaque, 2f);
    //    renderer.material.color = objectColor;
    //    yield return new WaitForEndOfFrame();
    //}
}
