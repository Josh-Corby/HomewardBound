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
    public static event Action OnObjectLengthChange;
    public BuildType Type;

    public BoxCollider CurrentTrigger;

    [SerializeField]
    public int ObjectLength;

    [SerializeField]
    private List<BuildObjectTrigger> _objectColliders = new List<BuildObjectTrigger>();

    [SerializeField]

    private MeshRenderer _renderer;

    [SerializeField]
    private Color _baseColour;

    [SerializeField]
    private bool _isBeingBuilt;

    [SerializeField]
    private bool _isTriggerNotColliding;


    public int StickRefundValue;
    public int RockRefundValue;
    public int StringRefundValue;

    private Material _material;

    private readonly float _objectBuildingAlpha = 1.1f;
    private readonly float _objectBuiltAlpha = 2f;
    private bool _isBuilt;

    [SerializeField]
    private GameObject[] _bridgeEndPoints;
    [SerializeField]
    private GameObject[] _bridgeLandPoints;

    [SerializeField]
    private GameObject _bridgeEndPoint;
    [SerializeField]
    private GameObject _bridgeLandPoint;

    private bool _isMarking;
    [SerializeField]
    private LayerMask _mask;

    [SerializeField]
    private GameObject _landingMarker;
    private void Awake()
    {
        _material = _renderer.material;
        _baseColour = _material.GetColor("_colour");
        for (int i = 1; i < _objectColliders.Count; i++)
        {
            _objectColliders[i].transform.gameObject.SetActive(false);
        }
        CurrentTrigger = _objectColliders[0].gameObject.GetComponent<BoxCollider>();
    }

    private void Start()
    {
        _isBuilt = false;
        ObjectLength = 0;
        ChangeChangeValueOfMaterial(_objectBuildingAlpha);
        if (Type == BuildType.BRIDGE)
        {
            UpdateLandingMarker(ObjectLength);
        }

        if (Type == BuildType.LADDER)
        {
            _isMarking = true;
        }
    }
    void Update()
    {
        if (UI.paused) return;

        BM.CollisionCheck = _isTriggerNotColliding;

        _isBeingBuilt = gameObject == BM.BuildingObject;

        if (_isBeingBuilt == false)
        {
            ObjectBuilt();
        }

        if (_isBeingBuilt)
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                //Debug.Log("Mouse up");
                if (ObjectLength == _objectColliders.Count)
                {
                    return;
                }
                ObjectLength += 1;
                OnObjectLengthChange();

                BM.SetMaterialCosts(2, ObjectLength);

                if (CurrentTrigger != null)
                {
                    CurrentTrigger.gameObject.SetActive(false);
                }
                CurrentTrigger = _objectColliders[ObjectLength - 1].transform.gameObject.GetComponent<BoxCollider>();

                CurrentTrigger.gameObject.SetActive(true);

            }
            if (Input.mouseScrollDelta.y < 0)
            {
                if (ObjectLength <= 1)
                {
                    return;
                }
                //Debug.Log("Mouse down");
                if (CurrentTrigger == null) return;
                ObjectLength -= 1;
                OnObjectLengthChange();

                BM.SetMaterialCosts(2, ObjectLength);
                CurrentTrigger.gameObject.SetActive(false);
                CurrentTrigger = _objectColliders[ObjectLength - 1].transform.gameObject.GetComponent<BoxCollider>();
                CurrentTrigger.gameObject.SetActive(true);

            }
            if (BM.MaterialsCheck)
            {
                if (_isTriggerNotColliding)
                {
                    ChangeColourOfObject(Color.blue);

                    if (TPM.groundState == GroundStates.Grounded)
                    {
                        ChangeColourOfObject(Color.blue);

                        if (!BM.OnBuildObject)
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

            ObjectLength = Mathf.Clamp(ObjectLength, 1, _objectColliders.Count);
        }


        if (Type == BuildType.BRIDGE)
        {
            UpdateLandingMarker(ObjectLength - 1);
            if (_isMarking)
            {
                LandingMarker();
            }
        }
    }
    public void CanObjectBeBuilt(bool triggerCollision)
    {
        _isTriggerNotColliding = triggerCollision;
    }
    private void ChangeColourOfObject(Color colour)
    {
        _material.SetColor("_colour", colour);
    }
    private void ObjectBuilt()
    {
        if (!_isBuilt)
        {
            ChangeChangeValueOfMaterial(_objectBuiltAlpha);
            ChangeColourOfObject(_baseColour);
            _isBuilt = true;
            _isMarking = false;
            _landingMarker.SetActive(false);
        }
    }
    private void ChangeChangeValueOfMaterial(float alpha)
    {
        //Debug.Log("Alpha changed");
        _material.SetFloat("_alphaValue", alpha);
    }
    public void RefundMaterials()
    {
        GM.AddMaterials(StickRefundValue, RockRefundValue, StringRefundValue);
    }
    private void UpdateLandingMarker(int value)
    {
        _isMarking = false;
        if (_bridgeLandPoint != null)
        {

            _bridgeLandPoint.SetActive(false);
        }

        if (_bridgeEndPoint != null)
        {

            _bridgeEndPoint.SetActive(false);
        }
        _bridgeEndPoint = _bridgeEndPoints[value];
        _bridgeLandPoint = _bridgeLandPoints[value];
        _bridgeEndPoint.SetActive(true);
        _bridgeLandPoint.SetActive(true);
        _isMarking = true;
    }
    private void LandingMarker()
    {
        Physics.Raycast(_bridgeEndPoint.transform.position, _bridgeLandPoint.transform.position - _bridgeEndPoint.transform.position, out RaycastHit hit, Mathf.Infinity, _mask);
        _landingMarker.transform.position = hit.point;
    }
}

