using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildObjects
{
    Bridge, 
    Ladder,
    Glider
}

public class BuildManager : GameBehaviour<BuildManager>
{
    public bool haveGlider;
    public bool isBuilding;

    private bool pebbleCheck;
    private bool stickCheck;
    private bool mushroomCheck;

    private int pebbleCost;
    private int stickCost;
    private int mushroomCost;


    [SerializeField] private GameObject buildZone;

    private GameObject prefabToSpawn;

    #region Build Prefabs
    [SerializeField] private GameObject ladderPrefab;
    [SerializeField] private GameObject bridgePrefab;
    #endregion

    void Start()
    {
        haveGlider = false;
    }

    private void Update()
    {
        if (IM.interact_Input && isBuilding)
        {
            Debug.Log("Object Built");
            buildZone.transform.DetachChildren();
            IZ.Toggle(true);
            isBuilding = false;
        }
    }
    public void BuildItem(int value)
    {
        switch ((BuildObjects)value)
        {
            case BuildObjects.Ladder:
                LadderCheck();
                prefabToSpawn = LadderCheck() ? ladderPrefab : null;
                isBuilding = LadderCheck();
                SubtractCost();
                UI.BuildMenuToggle();
                Instantiate(prefabToSpawn, buildZone.transform);
                break;

            case BuildObjects.Bridge:
                BridgeCheck();

                prefabToSpawn = BridgeCheck() ? bridgePrefab : null;
                isBuilding = BridgeCheck();
                SubtractCost();
                UI.BuildMenuToggle();
                Instantiate(prefabToSpawn, buildZone.transform);
                break;

            case BuildObjects.Glider:
                GliderCheck();   
                haveGlider = GliderCheck();
                SubtractCost();
                UI.BuildMenuToggle();
                break;
        }
        
        IZ.Toggle(true);
        IZ.DisableOutline();
    }

    #region Materials Comparisons

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

    public bool GliderCheck()
    {
        pebbleCost = 1;
        stickCost = 2;
        mushroomCost = 3;
        return CompareChecks();
    }

    private bool CompareChecks()
    {
        pebbleCheck = GM.pebblesCollected >= pebbleCost ? pebbleCheck = true : pebbleCheck = false;
        stickCheck = GM.sticksCollected >= stickCost ? stickCheck = true : stickCheck = false;
        mushroomCheck = GM.mushroomsCollected >= mushroomCost ? mushroomCheck = true : mushroomCheck = false;

        if (pebbleCheck == true && stickCheck == true && mushroomCheck)
            return true;
        else
            return false;  
    }

    private void SubtractCost()
    {
        GM.pebblesCollected -= pebbleCost;
        GM.sticksCollected -= stickCost;
        GM.mushroomsCollected -= mushroomCost;
        UI.UpdateMaterialsCollected();
    }
    #endregion
}
