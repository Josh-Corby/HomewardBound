using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildObjects
{
    Pickaxe,
    Ladder,
    Bridge,
    Bonfire,
    Slingshot,
    Ammo,
    GrappleHook,
    Glider
}
public class BuildManager : GameBehaviour<BuildManager>
{

    [Header("Build Checks")]
    public bool isBuilding;
    public bool canBuild;
    public bool collisionCheck;
    public bool onBuildObject;

    [Header("Crafting Checks")]
    private bool pebbleCheck;
    private bool stickCheck;
    private bool stringCheck;
    public bool materialsCheck;


    [Header("Craft Costs")]
    public int rockCost;
    public int stickCost;
    public int stringCost;

    [Header("Build Prefabs")]
    #region Build Prefabs
    [SerializeField]
    private GameObject ladderPrefab;
    [SerializeField]
    private GameObject bridgePrefab;
    [SerializeField]
    private GameObject bonfirePrefab;
    #endregion



    [SerializeField]
    private GameObject buildZone;

    public GameObject prefabToSpawn;


    public GameObject buildingObject;



    private int currentBuildObject_Index;

    private void Start()
    {
        PlayerManager.OnToolSelected += ToolSelectListen;
        GameManager.OnMaterialsUpdated += RunMaterialChecks;
        ObjectBuild.OnObjectLengthChange += RunMaterialChecks;
    }
    private void Update()
    {
        // If the player isn't building cancel the build input
        if (!isBuilding)
        {
            if (IM.cancel_Input)
            {
                IM.cancel_Input = false;
                return;
            }
        }
        if (isBuilding)
        {
            if (IM.cancel_Input)
            {
                if (prefabToSpawn != null)
                {
                    CancelBuilding();
                }
            }

            if (TPM.groundState != GroundStates.Grounded || onBuildObject)
            {
                canBuild = false;
                return;
            }
            if(TPM.groundState == GroundStates.Grounded)
            {
                canBuild = materialsCheck;
            }           
           
        }

        // Checks for if player can build
        if (Input.GetKeyDown(KeyCode.Mouse0) && isBuilding && !UI.paused)
        {
            // If material comparisons return true
            if (materialsCheck && canBuild && collisionCheck && !onBuildObject)
            {
   
                // Detach object from buildzone
                buildZone.transform.DetachChildren();

                // Reactivate Interaction Zone
                IZ.Toggle(true);
                buildingObject.GetComponent<ObjectBuild>().currentTrigger.isTrigger = false;
                SetObjectValue(buildingObject.GetComponent<ObjectBuild>());


                if (buildingObject.GetComponent<BuildObjectRB>() != null)
                {
                    buildingObject.gameObject.GetComponent<BuildObjectRB>().UnFreezeConstraints();
                    buildingObject.gameObject.GetComponent<BuildObjectRB>().frozen = false;
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
        objectBuilt.stick_Refund_Value = stickCost/2;
        objectBuilt.rock_Refund_Value = rockCost/2;
        objectBuilt.string_Refund_Value = stringCost/2;
    }

    private void ToolSelectListen(int buildObjectIndex)
    {
        if(buildObjectIndex == currentBuildObject_Index)
        {
            UI.DeselectHotbarOutline();
            CancelBuilding();
            currentBuildObject_Index = -1;
            return;
        }

        if (buildObjectIndex >= 1 && buildObjectIndex <= 2)
        {
            BuildItem(buildObjectIndex);
            currentBuildObject_Index = buildObjectIndex;
        }

        if(buildObjectIndex == 3)
        {
            CancelBuilding();
            currentBuildObject_Index = buildObjectIndex;
        }
    }

    private void ResetBuildObject()
    {
        // Reset manager bools
        buildingObject = null;
        prefabToSpawn = null;
        canBuild = false;
        isBuilding = false;
        currentBuildObject_Index = -1;
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
                prefabToSpawn = ladderPrefab;
                SetMaterialCosts(value, 1);
                StartCoroutine(BuildObject());
                break;

            case BuildObjects.Bridge:
                prefabToSpawn = bridgePrefab;
                SetMaterialCosts(value, 1);
                StartCoroutine(BuildObject());
                break;

            case BuildObjects.Bonfire:
                prefabToSpawn = bonfirePrefab;
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
        Destroy(buildingObject);
        buildingObject = null;
        // Wait a frame for other functions and updates to process that object has been destroyed
        yield return new WaitForEndOfFrame();

        // Instantiate object as a child of buildZone
        GameObject BuildObject = Instantiate(prefabToSpawn, buildZone.transform);
        buildingObject = BuildObject;
        //UI.BuildMenuToggle();
        isBuilding = true;
        materialsCheck = CompareChecks();
    }

    public void CancelBuilding()
    {
        
        if (buildingObject != null)
        {
            Destroy(buildingObject);
        }
        
        prefabToSpawn = null;
        canBuild = false;
        isBuilding = false;
        currentBuildObject_Index = -1;
    }
    public void SetMaterialCosts(int index, int costMultiplier)
    {
        switch ((BuildObjects)index)
        {
            case BuildObjects.Ladder:
                rockCost = 2;
                stickCost = 2;
                stringCost = 2;
                break;
            case BuildObjects.Bridge:
                rockCost = 2 * costMultiplier;
                stickCost = 2 * costMultiplier;
                stringCost = 2 * costMultiplier;
                RunMaterialChecks();
                break;
            case BuildObjects.Bonfire:
                rockCost = 3;
                stickCost = 3;
                stringCost = 3;
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
        if (isBuilding)
        {
            materialsCheck = CompareChecks();
        }
    }
    private bool CompareChecks()
    {
        //Debug.Log("Comparing materials");

        pebbleCheck = GM.rocksCollected >= rockCost;
        stickCheck = GM.sticksCollected >= stickCost;
        stringCheck = GM.stringCollected >= stringCost;

        if (pebbleCheck == true && stickCheck == true && stringCheck)
            return true;
        else
            return false;
    }
    /// <summary>
    /// Subtract the cost of a build object from the player when building is completed
    /// </summary>
    private void SubtractCost()
    {
        GM.rocksCollected -= rockCost;
        GM.sticksCollected -= stickCost;
        GM.stringCollected -= stringCost;
        UI.UpdateMaterialsCollected();
    }
    /// <summary>
    /// Add the cost of a build object back to the player when building is cancelled
    /// </summary>
    private void AddCost()
    {
        GM.rocksCollected += rockCost;
        GM.sticksCollected += stickCost;
        GM.stringCollected += stringCost;
        UI.UpdateMaterialsCollected();
    }

}