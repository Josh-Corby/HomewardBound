using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BridgeBuild : GameBehaviour
{
    [SerializeField]
    private GameObject currentExtension;
    [SerializeField]
    private int extensionCount;
    [SerializeField]
    private List<BuildObjectTrigger> BridgeSegmentTriggers = new List<BuildObjectTrigger>();
    public bool[] collisionChecks;
    private MeshRenderer renderer;
    [SerializeField]
    private Color baseColour;
    [SerializeField]
    private bool isBeingBuilt;
    [SerializeField]
    private bool canBuild;

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        baseColour = renderer.material.color;
        extensionCount = 0;
        currentExtension = null;

        for (int i = 1; i < BridgeSegmentTriggers.Count; i++)
        {
            BridgeSegmentTriggers[i].transform.parent.gameObject.SetActive(false);
        }
    }
    public void CheckSegmentCollisions(BuildObjectTrigger trigger)
    {
        for (int i = 0; i < extensionCount; i++)
        {
            //if (BridgeSegmentTriggers[i].enabled == false) return;
            //Debug.Log(BridgeSegmentTriggers[i]);
            if (BridgeSegmentTriggers[i] == trigger)
            {
                //Debug.Log("Collisions updated");
                collisionChecks[i] = trigger.canBuild;
            }
        }
        for (int i = 0; i < extensionCount; i++)
        {
            collisionChecks[i] = BridgeSegmentTriggers[i].canBuild;

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
            ChangeColourOfBridge(baseColour);
        }

        if (isBeingBuilt == true)
        {
            if (BM.canBuild)
            {
                ChangeColourOfBridge(Color.blue);
            }

            if (!BM.canBuild)
            {
                ChangeColourOfBridge(Color.red);
            }
        }


        extensionCount = Mathf.Clamp(extensionCount, 1, BridgeSegmentTriggers.Count);
        if (this.gameObject == BM.buildingObject)
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                if (extensionCount == BridgeSegmentTriggers.Count)
                {
                    return;
                }
                extensionCount += 1;
                BM.BridgeCheck(extensionCount);

                currentExtension = BridgeSegmentTriggers[extensionCount - 1].transform.parent.gameObject;
                currentExtension.SetActive(true);
          
            }
            if (Input.mouseScrollDelta.y < 0)
            {
                if (currentExtension == null) return;

                currentExtension.SetActive(false);
                extensionCount -= 1;
                BM.BridgeCheck(extensionCount);

                if (extensionCount <= 1)
                {
                    currentExtension = null;
                    return;
                }
                currentExtension = BridgeSegmentTriggers[extensionCount - 1].transform.parent.gameObject;
            }
        }

    }


    private void ChangeColourOfBridge(Color colour)
    {

        renderer.material.color = colour;

        for (int i = 1; i < BridgeSegmentTriggers.Count; i++)
        {
            BridgeSegmentTriggers[i].transform.parent.gameObject.GetComponent<MeshRenderer>().material.color = colour;
        }
        //foreach (BuildObjectTrigger extension in BridgeSegmentTriggers)
        //{
        //    extension.transform.parent.gameObject.GetComponent<MeshRenderer>().material.color = colour;
        //}
    }
}

