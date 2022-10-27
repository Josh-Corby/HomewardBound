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
    private bool segmentCollisionCheck;
    [SerializeField]
    private BridgeMeshManager bridgeMeshManager;
    [SerializeField]
    private BuildObjectTrigger trigger;

    private void Awake()
    {
        trigger = GetComponentInChildren<BuildObjectTrigger>();
        baseColour = renderer.material.color;
        extensionCount = 0;
        currentExtension = null;
        for (int i = 1; i < ObjectSegmentTriggers.Count; i++)
        {
            ObjectSegmentTriggers[i].transform.gameObject.SetActive(false);
        }
    }


    void Update()
    {
        if (UI.paused) return;

        BM.collisionCheck = segmentCollisionCheck;

        isBeingBuilt = gameObject == BM.buildingObject;


        if (isBeingBuilt == false)
        {
            ChangeColourOfObject(baseColour);
        }

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
            if (BM.materialsCheck)
            {
                if (segmentCollisionCheck)
                {
                    ChangeColourOfObject(Color.blue);

                    if(TPM.groundState == GroundStates.Grounded)
                    {
                        ChangeColourOfObject(Color.blue);
                    }
                    else
                    {
                        ChangeColourOfObject(Color.red);
                    }
                }
                else
                    ChangeColourOfObject(Color.red);


            }

            else 
            {
                ChangeColourOfObject(Color.red);
            }

            //    if (BM.materialsCheck)
            //    {
            //        if (BM.canBuild)
            //        {
            //            if (trigger.collisionCheck)
            //            {
            //                ChangeColourOfObject(Color.blue);

            //                if(TPM.groundState == GroundStates.Grounded)
            //                {
            //                    ChangeColourOfObject(Color.blue);
            //                }

            //                if(TPM.groundState != GroundStates.Grounded)
            //                {
            //                    ChangeColourOfObject(Color.red);
            //                }
            //            }
            //            else if (!trigger.collisionCheck)
            //                ChangeColourOfObject(Color.red);
            //        }         
            //    }

            //    else
            //        ChangeColourOfObject(Color.red);
            //}
            extensionCount = Mathf.Clamp(extensionCount, 1, ObjectSegmentTriggers.Count);

        }
    }
    public void CheckSegmentCollisions(BuildObjectTrigger trigger)
    {
        for (int i = 0; i < extensionCount; i++)
        {
            //if (BridgeSegmentTriggers[i].enabled == false) return;
            //Debug.Log(ObjectSegmentTriggers[i]);
            if (ObjectSegmentTriggers[i] == trigger)
            {
                //Debug.Log("Collisions updated");
                collisionChecks[i] = trigger.collisionCheck;
            }
        }
        for (int i = 0; i < extensionCount; i++)
        {
            collisionChecks[i] = ObjectSegmentTriggers[i].collisionCheck;

            //Debug.Log(collisionChecks[i]);
            if (collisionChecks[i] == false)
            {
                //Debug.Log("cant build bridge");
                segmentCollisionCheck = false;
                return;
            }
        }
        segmentCollisionCheck = true;
    }



    private void ChangeColourOfObject(Color colour)
    {
        renderer.material.color = colour;
    }

}

