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

    [Header("Crafting Checks")]
    private bool pebbleCheck;
    private bool stickCheck;
    private bool mushroomCheck;
    public bool materialsCheck;


    [Header("Craft Costs")]
    [SerializeField]
    private int pebbleCost;
    [SerializeField]
    private int stickCost;
    [SerializeField]
    private int mushroomCost;

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
    //private Color buildObjectBaseColour;
    [SerializeField]
    private Color buildObjectColour;
    private MeshRenderer buildObjectRenderer;



    private void Start()
    {
        PlayerManager.OnToolSelected += ToolSelectListen;
        GameManager.OnMaterialsUpdated += RunMaterialChecks;
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
            canBuild = materialsCheck;
            if (IM.cancel_Input)
            {
                if (prefabToSpawn != null)
                {
                    CancelBuilding();
                }
            }
        }

        // Checks for if player can build
        if (Input.GetKeyDown(KeyCode.Mouse0) && isBuilding && !UI.paused)
        {
            if (TPM.groundState == GroundStates.Airborne || canBuild == false)
            {
                IM.rClick_Input = false;
                return;
            }

            // If material comparisons return true
            else if (canBuild)
            {
                Debug.Log("Object Built");

                // Detach object from buildzone
                buildZone.transform.DetachChildren();

                // Reactivate Interaction Zone
                IZ.Toggle(true);
                buildingObject.GetComponent<BoxCollider>().enabled = true;

                if (buildingObject.GetComponent<BuildObjectRB>() != null)
                {
                    buildingObject.gameObject.GetComponent<BuildObjectRB>().UnFreezeConstraints();
                    buildingObject.gameObject.GetComponent<BuildObjectRB>().frozen = false;
                }
                SubtractCost();
                ResetBuildObject();
                return;
            }
            return;
        }
    }

    private void ToolSelectListen(int buildObjectIndex)
    {
        if (buildObjectIndex >= 1 && buildObjectIndex <= 3)
        {
            BuildItem(buildObjectIndex);
        }
    }

    private void ResetBuildObject()
    {
        // Reset manager bools
        buildingObject = null;
        prefabToSpawn = null;
        canBuild = false;
        isBuilding = false;
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
    }
    public void SetMaterialCosts(int index, int costMultiplier)
    {
        switch ((BuildObjects)index)
        {
            case BuildObjects.Ladder:
                pebbleCost = 3;
                stickCost = 2;
                mushroomCost = 1;
                break;
            case BuildObjects.Bridge:
                pebbleCost = 1 * costMultiplier;
                stickCost = 1 * costMultiplier;
                mushroomCost = 1 * costMultiplier;
                break;
            case BuildObjects.Bonfire:
                pebbleCost = 3;
                stickCost = 3;
                mushroomCost = 3;
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
        if(isBuilding)
        CompareChecks();
    }
    private bool CompareChecks()
    {
        {
            pebbleCheck = GM.rocksCollected >= pebbleCost;
            stickCheck = GM.sticksCollected >= stickCost;
            mushroomCheck = GM.mushroomsCollected >= mushroomCost;

            if (pebbleCheck == true && stickCheck == true && mushroomCheck)
                return true;
            else
                return false;
        }
    }
        /// <summary>
        /// Subtract the cost of a build object from the player when building is completed
        /// </summary>
        private void SubtractCost()
        {
            GM.rocksCollected -= pebbleCost;
            GM.sticksCollected -= stickCost;
            GM.mushroomsCollected -= mushroomCost;
            UI.UpdateMaterialsCollected();
        }
        /// <summary>
        /// Add the cost of a build object back to the player when building is cancelled
        /// </summary>
        private void AddCost()
        {
            GM.rocksCollected += pebbleCost;
            GM.sticksCollected += stickCost;
            GM.mushroomsCollected += mushroomCost;
            UI.UpdateMaterialsCollected();
        }

    }