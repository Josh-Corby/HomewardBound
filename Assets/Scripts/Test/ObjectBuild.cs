using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ObjectBuild : GameBehaviour
{

    public static event Action OnObjectLengthChange;

    [SerializeField]
    private GameObject currentExtension;
    [SerializeField]
    public int extensionCount;
    [SerializeField]
    private List<BuildObjectTrigger> ObjectSegmentTriggers = new List<BuildObjectTrigger>();
    public bool[] collisionChecks;
    [SerializeField]
    private MeshRenderer renderer;
    [SerializeField]
    private Color baseColour;
    [SerializeField]
    private bool isBeingBuilt;
    [SerializeField]
    private bool canBuild;

    [SerializeField]
    private BridgeMeshManager bridgeMeshManager;

    private void Awake()
    {
        baseColour = renderer.material.color;
        extensionCount = 0;
        currentExtension = null;
        for (int i = 1; i < ObjectSegmentTriggers.Count; i++)
        {
            ObjectSegmentTriggers[i].transform.gameObject.SetActive(false);
        }
    }
    public void CheckSegmentCollisions(BuildObjectTrigger trigger)
    {
        for (int i = 0; i < extensionCount; i++)
        {
            //if (BridgeSegmentTriggers[i].enabled == false) return;
            //Debug.Log(BridgeSegmentTriggers[i]);
            if (ObjectSegmentTriggers[i] == trigger)
            {
                //Debug.Log("Collisions updated");
                collisionChecks[i] = trigger.canBuild;
            }
        }
        for (int i = 0; i < extensionCount; i++)
        {
            collisionChecks[i] = ObjectSegmentTriggers[i].canBuild;

            //Debug.Log(collisionChecks[i]);
            if (collisionChecks[i] == false)
            {
                //Debug.Log("cant build bridge");
                canBuild = false;
                return;
            }
        }
        canBuild = true;
    }
    void Update()
    {
        if (UI.paused) return;

        BM.canBuild = canBuild;

        isBeingBuilt = gameObject == BM.buildingObject;


        if (isBeingBuilt == false)
        {
            ChangeColourOfObject(baseColour);
        }

        if (isBeingBuilt == true)
        {
            if (BM.canBuild)
            {
                ChangeColourOfObject(Color.blue);
            }

            if (!BM.canBuild)
            {
                ChangeColourOfObject(Color.red);
            }
        }
        extensionCount = Mathf.Clamp(extensionCount, 1, ObjectSegmentTriggers.Count);

        if (isBeingBuilt)
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                //Debug.Log("Mouse up");
                if (extensionCount == ObjectSegmentTriggers.Count)
                {
                    return;
                }
                extensionCount += 1;
                OnObjectLengthChange();

                BM.SetMaterialCosts(2, extensionCount);

                currentExtension = ObjectSegmentTriggers[extensionCount - 1].transform.gameObject;
                currentExtension.SetActive(true);
          
            }
            if (Input.mouseScrollDelta.y < 0)
            {
                //Debug.Log("Mouse down");
                if (currentExtension == null) return;

                currentExtension.SetActive(false);
                extensionCount -= 1;
                OnObjectLengthChange();

                BM.SetMaterialCosts(2, extensionCount);

                if (extensionCount <= 1)
                {
                    currentExtension = null;
                    return;
                }
                currentExtension = ObjectSegmentTriggers[extensionCount - 1].transform.gameObject;
            }
        }

    }


    private void ChangeColourOfObject(Color colour)
    {

        renderer.material.color = colour;

        for (int i = 1; i < ObjectSegmentTriggers.Count; i++)
        {
            ObjectSegmentTriggers[i].transform.parent.gameObject.GetComponentInChildren<MeshRenderer>().material.color = colour;
        }
        //foreach (BuildObjectTrigger extension in BridgeSegmentTriggers)
        //{
        //    extension.transform.parent.gameObject.GetComponent<MeshRenderer>().material.color = colour;
        //}
    }

}

