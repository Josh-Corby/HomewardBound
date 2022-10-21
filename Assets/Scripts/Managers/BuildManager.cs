using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildObjects
{
    Pickaxe,
    Ladder,
    Bridge,
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


    [Header("Craft Costs")]
    private int pebbleCost;
    private int stickCost;
    private int mushroomCost;

    [Header("Build Prefabs")]
    #region Build Prefabs
    [SerializeField] private GameObject ladderPrefab;
    [SerializeField] private GameObject bridgePrefab;
    #endregion

    [SerializeField]
    private GameObject buildZone;

    public GameObject prefabToSpawn;

    [SerializeField]
    private GameObject buildObject;
    public GameObject buildingObject;
    private Color buildObjectBaseColour;
    [SerializeField]
    private Color buildObjectColour;
    private MeshRenderer buildObjectRenderer;


    private bool canRepeat;
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
            if (!canBuild)
            {

                buildObjectColour = Color.red;
                buildObjectColour.a = 0.1f;
                buildObjectRenderer.material.color = buildObjectColour;
            }

            if (canBuild)
            {

                buildObjectColour = Color.blue;
                buildObjectColour.a = 0.1f;
                buildObjectRenderer.material.color = buildObjectColour;

            }

            if (IM.cancel_Input)
            {
                if (prefabToSpawn != null)
                {
                    CancelBuilding();
                }
            }
        }

        // Checks for if player can build
        if (Input.GetKeyDown(KeyCode.Mouse1) && isBuilding && !UI.paused)
        {
            if (TPM.groundState == GroundStates.Airborne || canBuild == false)
            {
                IM.rClick_Input = false;
                return;
            }

            // If material comparisons return true
            else if (canBuild)
            {
                buildObjectRenderer.material.color = buildObjectBaseColour;
                //StartCoroutine(buildObject.LerpAlpha());
                Debug.Log("Object Built");

                // Detach object from buildzone
                buildZone.transform.DetachChildren();

                // Reactivate Interaction Zone
                IZ.Toggle(true);
                buildObject.GetComponent<BoxCollider>().enabled = true;
                buildObject.gameObject.GetComponent<BuildObjectRB>().UnFreezeConstraints();
                buildObject.gameObject.GetComponent<BuildObjectRB>().frozen = false;

                SubtractCost();
                ResetBuildObject();
                return;

                canRepeat = true;
                RepeatBuild();

                //ResetBuildObject();             
            }

            return;

        }
    }

    private void RepeatBuild()
    {
        if (canRepeat)
        {
            if (buildingObject.CompareTag("Bridge"))
            {
                //Debug.Log("Repeat bridge");
                ResetBuildObject();
                BuildItem(2);
                canRepeat = false;
                return;
            }

            if (buildingObject.CompareTag("Ladder"))
            {
                //Debug.Log("Repeat bridge");
                ResetBuildObject();
                BuildItem(1);
                canRepeat = false;
                return;
            }
        }

    }

    private void ResetBuildObject()
    {
        // Reset manager bools
        buildObject.GetComponent<BoxCollider>().enabled = true;
        buildingObject = null;
        buildObject = null;
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
                if (LadderCheck())
                {
                    OM.ChangeOutfits(1);
                    prefabToSpawn = ladderPrefab;
                }
                else
                {
                    OM.ChangeOutfits(4);
                    return;
                }
                StartCoroutine(BuildObject());
                break;

            case BuildObjects.Bridge:
                if (BridgeCheck())
                {
                    prefabToSpawn = bridgePrefab;
                    OM.ChangeOutfits(1);
                }
                else
                {
                    OM.ChangeOutfits(4);
                    return;
                }

                StartCoroutine(BuildObject());
                break;
                /*
                 * case BuildObjects.Ammo:
                 * AmmoCheck();
                 * SS.ammo += 5;
                 * SubtractCost();
                 * break;
                */
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
        buildObject = null;


        // Wait a frame for other functions and updates to process that object has been destroyed
        yield return new WaitForEndOfFrame();

        // Instantiate object as a child of buildZone
        Instantiate(prefabToSpawn, buildZone.transform);
        buildingObject = buildZone.transform.GetChild(0).gameObject;
        buildObjectRenderer = buildingObject.GetComponentInChildren<MeshRenderer>();
        buildObjectBaseColour = buildObjectRenderer.material.color;
        buildObject = buildingObject.transform.GetChild(0).gameObject;
        
        buildObject.GetComponentInChildren<BoxCollider>().enabled = false;
        //UI.BuildMenuToggle();
        isBuilding = true;
    }

    public void CancelBuilding()
    {
        if(buildingObject != null)
        {
            Destroy(buildingObject);
        }
  
        prefabToSpawn = null;
        canBuild = false;
        isBuilding = false;
    }

    #region Materials Comparisons
    /*
     * These functions define the costs of objects that can be built
     * After defining the cost it takes to build an object it runs a check to see if the object can be built
     * A bool is then returned telling the manager if the player can build or not
    */

    /*
    public bool PickaxeCheck()
    {
        pebbleCost = 3;
        stickCost = 3;
        mushroomCost = 0;
        return CompareChecks();
    }
    */

    /// <summary>
    /// Define the cost of the ladder, then runs a comparison function
    /// 
    /// </summary>
    /// <returns>Returns a bool that tells the manager if the object can be built </returns>
    public bool LadderCheck()
    {
        pebbleCost = 3;
        stickCost = 2;
        mushroomCost = 1;
        return CompareChecks();
    }

    /// <summary>
    /// Define the cost of the bridge, then runs a comparison function
    /// </summary>
    /// <returns>Returns a bool that tells the manager if the object can be built</returns>
    public bool BridgeCheck()
    {
        pebbleCost = 2;
        stickCost = 3;
        mushroomCost = 1;
        return CompareChecks();
    }
    /*
    public bool SlingshotCheck()
    {
        pebbleCost = 1;
        stickCost = 2;
        mushroomCost = 3;
        return CompareChecks();
    }

    public bool AmmoCheck()
    {
        pebbleCost = 2;
        stickCost = 0;
        mushroomCost = 0;
        return CompareChecks();
    }
    
    public bool GliderCheck()
    {
        pebbleCost = 1;
        stickCost = 2;
        mushroomCost = 3;
        return CompareChecks();
    }
    public bool GrappleHookCheck()
    {
        pebbleCost = 2;
        stickCost = 2;
        mushroomCost = 2;
        return CompareChecks();
    }
    */


    /// <summary>
    /// Compare object price with materials player has
    /// </summary>
    /// <returns>Returns a bool that says if an object can be built or not</returns>
    private bool CompareChecks()
    {
        pebbleCheck = GM.rocksCollected >= pebbleCost ? pebbleCheck = true : pebbleCheck = false;
        stickCheck = GM.sticksCollected >= stickCost ? stickCheck = true : stickCheck = false;
        mushroomCheck = GM.mushroomsCollected >= mushroomCost ? mushroomCheck = true : mushroomCheck = false;

        if (pebbleCheck == true && stickCheck == true && mushroomCheck)
            return true;
        else
            return false;
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
    #endregion
}