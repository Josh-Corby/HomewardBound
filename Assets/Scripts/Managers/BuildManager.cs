using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BuildObjects
{
    Ladder,
    Bridge,
    Slingshot
}
public class BuildManager : GameBehaviour<BuildManager>
{
    public static event Action<bool> OnCanBuildStatus = null;

    [Header("Build Checks")]
    public bool IsBuilding;
    public bool CanBuild;
    public bool CollisionCheck;
    public bool OnBuildObject;

    [Header("Crafting Checks")]
    private bool _pebbleCheck;
    private bool _stickCheck;
    private bool _stringCheck;
    public bool MaterialsCheck;
    public bool AllChecks;


    [Header("Craft Costs")]
    public int RockCost;
    public int StickCost;
    public int StringCost;

    [Header("Build Prefabs")]

    [SerializeField]
    private GameObject _ladderPrefab;
    [SerializeField]
    private GameObject _bridgePrefab;




    [SerializeField]
    private GameObject _buildZone;

    [HideInInspector]
    public GameObject PrefabToSpawn;

    [HideInInspector]
    public GameObject BuildingObject;
    private int _currentBuildObjectIndex;

    private void Start()
    {
        PlayerManager.OnToolSelected += ToolSelectListen;
        GameManager.OnMaterialsUpdated += RunMaterialChecks;
        ObjectBuild.OnObjectLengthChange += RunMaterialChecks;
    }
    private void Update()
    {
        // If the player isn't building cancel the build input
        if (!IsBuilding)
        {
            if (IM.CancelInput)
            {
                IM.CancelInput = false;
                return;
            }
        }
        if (IsBuilding)
        {
            if (IM.CancelInput)
            {
                if (PrefabToSpawn != null)
                {
                    CancelBuilding();
                }
            }

            if (TPM.groundState != GroundStates.Grounded || OnBuildObject)
            {
                CanBuild = false;
            }
            if (TPM.groundState == GroundStates.Grounded)
            {
                CanBuild = MaterialsCheck;
            }

            if (TPM.groundState == GroundStates.Grounded && CanBuild && CollisionCheck && !OnBuildObject && !UI.paused)
            {

                AllChecks = true;
                OnCanBuildStatus(true);
            }

            if(TPM.groundState != GroundStates.Grounded || !CanBuild || !CollisionCheck || OnBuildObject || UI.paused)
            {
                AllChecks = false;
                OnCanBuildStatus(false);
            }
        }

        // Checks for if player can build
        if (Input.GetKeyDown(KeyCode.Mouse0) && IsBuilding && !UI.paused)
        {
            // If material comparisons return true
            if (MaterialsCheck && CanBuild && CollisionCheck && !OnBuildObject)
            {

                // Detach object from buildzone
                _buildZone.transform.DetachChildren();

                // Reactivate Interaction Zone
                IZ.Toggle(true);
                BuildingObject.GetComponent<ObjectBuild>().CurrentTrigger.isTrigger = false;
                SetObjectValue(BuildingObject.GetComponent<ObjectBuild>());


                if (BuildingObject.GetComponent<BuildObjectRB>() != null)
                {
                    BuildingObject.gameObject.GetComponent<BuildObjectRB>().UnFreezeConstraints();
                    BuildingObject.gameObject.GetComponent<BuildObjectRB>().Frozen = false;
                }
                UI.DeselectHotbarOutline();
                SubtractCost();
                ResetBuildObject();
                return;
            }
            return;
        }
    }

    private void SetObjectValue(ObjectBuild objectBuilt)
    {
        objectBuilt.StickRefundValue = StickCost / 2;
        objectBuilt.RockRefundValue = RockCost / 2;
        objectBuilt.StringRefundValue = StringCost / 2;
    }

    private void ToolSelectListen(int buildObjectIndex)
    {
        buildObjectIndex -= 1;
        if (buildObjectIndex == _currentBuildObjectIndex)
        {
            UI.DeselectHotbarOutline();
            CancelBuilding();
            _currentBuildObjectIndex = -1;
            return;
        }

        if (buildObjectIndex >= 0 && buildObjectIndex <= 1)
        {
            BuildItem(buildObjectIndex);
            _currentBuildObjectIndex = buildObjectIndex;
            return;
        }

        if (buildObjectIndex == 3)
        {
            CancelBuilding();
            _currentBuildObjectIndex = buildObjectIndex;
            return;
        }
    }

    private void ResetBuildObject()
    {
        // Reset manager bools
        BuildingObject = null;
        PrefabToSpawn = null;
        CanBuild = false;
        IsBuilding = false;
        _currentBuildObjectIndex = -1;
    }

    /// <summary>
    /// Assigns objects to be spawned with switch statements
    /// </summary>
    /// <param name="value"> Defines what case of the BuildObjects enum is run</param>
    public void BuildItem(int value)
    {
        switch ((BuildObjects)value)
        {
            case BuildObjects.Ladder:
                PrefabToSpawn = _ladderPrefab;
                SetMaterialCosts(value, 1);
                StartCoroutine(BuildObject());
                break;

            case BuildObjects.Bridge:
                PrefabToSpawn = _bridgePrefab;
                SetMaterialCosts(value, 1);
                StartCoroutine(BuildObject());
                break;


        }
        IZ.Toggle(true);
        IZ.DisableInteractions();
    }


    /// <summary>
    /// Assigns object to be built and instantiates it as a parent of the buildZone
    /// </summary>
    IEnumerator BuildObject()
    {
        // Destroy any objects that shouldnt be there and make buildingobject null for function
        Destroy(BuildingObject);
        BuildingObject = null;
        // Wait a frame for other functions and updates to process that object has been destroyed
        yield return new WaitForEndOfFrame();

        // Instantiate object as a child of buildZone
        GameObject BuildObject = Instantiate(PrefabToSpawn, _buildZone.transform);
        BuildingObject = BuildObject;
        //UI.BuildMenuToggle();
        IsBuilding = true;
        MaterialsCheck = CompareChecks();
    }

    public void CancelBuilding()
    {
        UI.DeselectHotbarOutline();
        if (BuildingObject != null)
        {
            Destroy(BuildingObject);
        }

        PrefabToSpawn = null;
        CanBuild = false;
        IsBuilding = false;
        _currentBuildObjectIndex = -1;
        
    }
    public void SetMaterialCosts(int index, int costMultiplier)
    {
        switch ((BuildObjects)index)
        {
            case BuildObjects.Ladder:
                RockCost = 2;
                StickCost = 2;
                StringCost = 2;
                break;
            case BuildObjects.Bridge:
                RockCost = 2 * costMultiplier;
                StickCost = 2 * costMultiplier;
                StringCost = 2 * costMultiplier;
                RunMaterialChecks();
                break;

        }
    }
    /// <summary>
    /// Compare object price with materials player has
    /// </summary>
    /// <returns>Returns a bool that says if an object can be built or not</returns>
    /// 

    private void RunMaterialChecks()
    {
        if (IsBuilding)
        {
            MaterialsCheck = CompareChecks();
        }
    }
    private bool CompareChecks()
    {
        //Debug.Log("Comparing materials");

        _pebbleCheck = GM.RocksCollected >= RockCost;
        _stickCheck = GM.SticksCollected >= StickCost;
        _stringCheck = GM.StringCollected >= StringCost;

        if (_pebbleCheck == true && _stickCheck == true && _stringCheck)
            return true;
        else
            return false;
    }
    /// <summary>
    /// Subtract the cost of a build object from the player when building is completed
    /// </summary>
    private void SubtractCost()
    {
        GM.RocksCollected -= RockCost;
        GM.SticksCollected -= StickCost;
        GM.StringCollected -= StringCost;
        UI.UpdateMaterialsCollected();
    }
    /// <summary>
    /// Add the cost of a build object back to the player when building is cancelled
    /// </summary>
    private void AddCost()
    {
        GM.RocksCollected += RockCost;
        GM.SticksCollected += StickCost;
        GM.StringCollected += StringCost;
        UI.UpdateMaterialsCollected();
    }

}