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
    
    public bool isBuilding;
    public bool canBuild;

    private bool pebbleCheck;
    private bool stickCheck;
    private bool mushroomCheck;

    private int pebbleCost;
    private int stickCost;
    private int mushroomCost;


    [SerializeField] private GameObject buildZone;

    private GameObject prefabToSpawn;

    public GameObject buildingObject;
    private Color objectColor;

    #region Build Prefabs
    [SerializeField] private GameObject ladderPrefab;
    [SerializeField] private GameObject bridgePrefab;
    #endregion


    private void Update()
    {
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
            if(OM.outfits != Outfits.Builder)
            {
                CancelBuilding();
            }

            if (IM.cancel_Input)
            {
                if(prefabToSpawn != null)
                {
                    CancelBuilding();
                }
            }
        }
        if (IM.interact_Input && isBuilding)
        {
            if (TPM.groundState == GroundStates.Airborne)
            {
                IM.interact_Input = false;
                return;
            }
            if (canBuild)
            {
                objectColor.a = 1f;
                buildingObject.GetComponent<MeshRenderer>().material.color = objectColor;
                Debug.Log("Object Built");
                buildZone.transform.DetachChildren();
                canBuild = false;
                IZ.Toggle(true);
                isBuilding = false;
                SubtractCost();
                buildingObject.gameObject.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionY;
                buildingObject = null;
                
            }

            if (!canBuild)
            {
                IM.interact_Input = false;
                return;
            }
            
        }
    }
    public void BuildItem(int value)
    {
        switch ((BuildObjects)value)
        {
            case BuildObjects.Pickaxe:
                PickaxeCheck();
                GM.havePickaxe = GliderCheck();
                SubtractCost();
                UI.BuildMenuToggle();
                break;

            case BuildObjects.Ladder:
                LadderCheck();
                prefabToSpawn = LadderCheck() ? ladderPrefab : null;
                isBuilding = LadderCheck();
                StartCoroutine(BuildObject());
                canBuild = true;
                break;

            case BuildObjects.Bridge:
                BridgeCheck();
                prefabToSpawn = BridgeCheck() ? bridgePrefab : null;
                isBuilding = BridgeCheck();
                StartCoroutine(BuildObject());
                canBuild = true;
                break;

            case BuildObjects.Slingshot:
                SlingshotCheck();
                GM.haveSlingshot = SlingshotCheck();
                SubtractCost();
                UI.BuildMenuToggle();
                break;

            case BuildObjects.Ammo:
                AmmoCheck();
                SS.ammo += 5;
                SubtractCost();
                break;

            case BuildObjects.Glider:
                GliderCheck();   
                GM.haveGlider = GliderCheck();
                SubtractCost();
                UI.BuildMenuToggle();
                break;
            case BuildObjects.GrappleHook:
                GrappleHookCheck();
                GM.haveGrappleHook = GrappleHookCheck();
                SubtractCost();
                UI.BuildMenuToggle();
                break;
                

        }
        
        IZ.Toggle(true);
        IZ.DisableInteractions();
    }

    IEnumerator BuildObject()
    {
        Destroy(buildingObject);
        buildingObject = null;
        
        yield return new WaitForEndOfFrame();
        Instantiate(prefabToSpawn, buildZone.transform);
        buildingObject = buildZone.transform.GetChild(0).gameObject;
        objectColor = buildingObject.GetComponent<MeshRenderer>().material.color;
        UI.BuildMenuToggle();
    }

    private void CancelBuilding()
    {
        Destroy(buildingObject);
        AddCost();
        prefabToSpawn = null;
        canBuild = false;
        isBuilding = false;
    }

    #region Materials Comparisons

    public bool PickaxeCheck()
    {
        pebbleCost = 3;
        stickCost = 3;
        mushroomCost = 0;
        return CompareChecks();
    }

    public bool LadderCheck()
    {
        pebbleCost = 3;
        stickCost = 2;
        mushroomCost = 1;
        return CompareChecks();
    }

    public bool BridgeCheck()
    {
        pebbleCost = 2;
        stickCost = 3;
        mushroomCost = 1;
        return CompareChecks();
    }

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

    private void SubtractCost()
    {
        GM.rocksCollected -= pebbleCost;
        GM.sticksCollected -= stickCost;
        GM.mushroomsCollected -= mushroomCost;
        UI.UpdateMaterialsCollected();
    }

    private void AddCost()
    {
        GM.rocksCollected += pebbleCost;
        GM.sticksCollected += stickCost;
        GM.mushroomsCollected += mushroomCost;
        UI.UpdateMaterialsCollected();
    }
    #endregion
}
