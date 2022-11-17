using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BuildType
{
    LADDER, BRIDGE
}
public class ObjectBuild : GameBehaviour
{
    public BuildType type;
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
    private bool isTriggerNotColliding;


    public int stick_Refund_Value;
    public int rock_Refund_Value;
    public int mushroom_Refund_Value;

    private Material material;

    private float objectBuildingAlpha = 1.1f;
    private float objectBuiltAlpha = 2f;
    private bool isBuilt;

    [SerializeField]
    private GameObject[] bridgeEndPoints;
    [SerializeField]
    private GameObject[] bridgeLandPoints;

    [SerializeField]
    private GameObject bridgeEndPoint;
    [SerializeField]
    private GameObject bridgeLandPoint;

    private bool isMarking;
    [SerializeField]
    private LayerMask mask;

    [SerializeField]
    private GameObject landingMarker;
    private void Awake()
    {
        material = renderer.material;
        baseColour = material.GetColor("_colour");
        for (int i = 1; i < ObjectColliders.Count; i++)
        {
            ObjectColliders[i].transform.gameObject.SetActive(false);
        }
        currentTrigger = ObjectColliders[0].gameObject.GetComponent<BoxCollider>();
    }

    private void Start()
    {
        isBuilt = false;
        objectLength = 0;
        ChangeChangeValueOfMaterial(objectBuildingAlpha);
        if (type == BuildType.BRIDGE)
        {
            UpdateLandingMarker(objectLength);
        }

        if(type == BuildType.LADDER)
        {
            isMarking = true;
        }
    }
    void Update()
    {
        if (UI.paused) return;

        BM.collisionCheck = isTriggerNotColliding;

        isBeingBuilt = gameObject == BM.buildingObject;

        if (isBeingBuilt == false)
        {
            ObjectBuilt();
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
                if (isTriggerNotColliding)
                {
                    ChangeColourOfObject(Color.blue);

                    if (TPM.groundState == GroundStates.Grounded)
                    {
                        ChangeColourOfObject(Color.blue);

                        if (!BM.onBuildObject)
                            ChangeColourOfObject(Color.blue);
                        else
                            ChangeColourOfObject(Color.red);
                    }
                    else
                        ChangeColourOfObject(Color.red);
                }
                else
                    ChangeColourOfObject(Color.red);
            }
            else
                ChangeColourOfObject(Color.red);

            objectLength = Mathf.Clamp(objectLength, 1, ObjectColliders.Count);
        }


        if (type == BuildType.BRIDGE)
        {
            UpdateLandingMarker(objectLength - 1);
            if (isMarking)
            {
                LandingMarker();
            }
        }
    }
    public void CanObjectBeBuilt(bool triggerCollision)
    {
        isTriggerNotColliding = triggerCollision;
    }
    private void ChangeColourOfObject(Color colour)
    {
        material.SetColor("_colour", colour);
    }
    private void ObjectBuilt()
    {
        if (!isBuilt)
        {
            ChangeChangeValueOfMaterial(objectBuiltAlpha);
            ChangeColourOfObject(baseColour);
            isBuilt = true;
            isMarking = false;
            landingMarker.SetActive(false);
        }
    }
    private void ChangeChangeValueOfMaterial(float alpha)
    {
        //Debug.Log("Alpha changed");
        material.SetFloat("_alphaValue", alpha);
    }
    public void RefundMaterials()
    {
        GM.AddMaterials(stick_Refund_Value, rock_Refund_Value, mushroom_Refund_Value);
    }
    private void UpdateLandingMarker(int value)
    {
        isMarking = false;
        bridgeLandPoint.SetActive(false);
        bridgeEndPoint.SetActive(false);
        bridgeEndPoint = bridgeEndPoints[value];
        bridgeLandPoint = bridgeLandPoints[value];
        bridgeEndPoint.SetActive(true);
        bridgeLandPoint.SetActive(true);
        isMarking = true;
    }
    private void LandingMarker()
    {
        Physics.Raycast(bridgeEndPoint.transform.position, bridgeLandPoint.transform.position);
        Physics.Raycast(bridgeEndPoint.transform.position, bridgeLandPoint.transform.position - bridgeEndPoint.transform.position, out RaycastHit hit, Mathf.Infinity, mask);
        landingMarker.transform.position = hit.point;
    }
}

