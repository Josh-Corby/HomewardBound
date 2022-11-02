using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ObjectBuild : GameBehaviour
{

    public static event Action OnObjectLengthChange;

    public BoxCollider currentTrigger;

    [SerializeField]
    public int objectLength;

    [SerializeField]
    private List<BuildObjectTrigger> ObjectColliders = new List<BuildObjectTrigger>();

    [SerializeField]

    private MeshRenderer renderer;

    [SerializeField]
    private Color baseColour;

    [SerializeField]
    private bool isBeingBuilt;

    [SerializeField]
    private bool isTriggerColliding;


    public int stick_Refund_Value;
    public int rock_Refund_Value;
    public int mushroom_Refund_Value;

    private void Awake()
    {
        baseColour = renderer.material.color;
        objectLength = 0;
        for (int i = 1; i < ObjectColliders.Count; i++)
        {
            ObjectColliders[i].transform.gameObject.SetActive(false);
        }
        currentTrigger = ObjectColliders[0].gameObject.GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (UI.paused) return;

        BM.collisionCheck = isTriggerColliding;

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
                if (objectLength == ObjectColliders.Count)
                {
                    return;
                }
                objectLength += 1;
                OnObjectLengthChange();

                BM.SetMaterialCosts(2, objectLength);

                if (currentTrigger != null)
                {
                currentTrigger.gameObject.SetActive(false);
                }
                currentTrigger = ObjectColliders[objectLength - 1].transform.gameObject.GetComponent<BoxCollider>();

                currentTrigger.gameObject.SetActive(true);

            }
            if (Input.mouseScrollDelta.y < 0)
            {
                if (objectLength <= 1)
                {
                    return;
                }
                //Debug.Log("Mouse down");
                if (currentTrigger == null) return;
                objectLength -= 1;
                OnObjectLengthChange();

                BM.SetMaterialCosts(2, objectLength);         
                currentTrigger.gameObject.SetActive(false);
                currentTrigger = ObjectColliders[objectLength - 1].transform.gameObject.GetComponent<BoxCollider>();
                currentTrigger.gameObject.SetActive(true);
            }
            if (BM.materialsCheck)
            {
                if (isTriggerColliding)
                {
                    ChangeColourOfObject(Color.blue);

                    if (TPM.groundState == GroundStates.Grounded)
                    {
                        ChangeColourOfObject(Color.blue);

                        if (!BM.onBuildObject)
                        {
                            ChangeColourOfObject(Color.blue);
                        }
                        else
                        {
                            ChangeColourOfObject(Color.red);
                        }
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
            objectLength = Mathf.Clamp(objectLength, 1, ObjectColliders.Count);
        }
    }
    public void CanObjectBeBuilt(bool triggerCollision)
    {
        isTriggerColliding = triggerCollision;
    }
    private void ChangeColourOfObject(Color colour)
    {
        renderer.material.color = colour;
    }

    public void RefundMaterials()
    {
        GM.AddMaterials(stick_Refund_Value, rock_Refund_Value, mushroom_Refund_Value);
    }
}

