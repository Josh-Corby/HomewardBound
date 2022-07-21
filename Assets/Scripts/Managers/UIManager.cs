using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : GameBehaviour<UIManager>
{
    public TMP_Text flashLightIntensity;
    public TMP_Text canBuild;
    public TMP_Text fallTimer;

    public TMP_Text smallRocksCollected;
    public TMP_Text sticksCollected;
    public TMP_Text mushroomsCollected;
    public TMP_Text pebblesCollected;
    public TMP_Text currentOutfit;

    public GameObject gameUI;
    public GameObject buildPanel;
    public bool buildPanelStatus;

    public GameObject[] BuildPanels;

    private GameObject currentBuildPanel;
    public Button buildPickaxeButton;
    public Button buildLadderButton;
    public Button buildBridgeButton;
    public Button buildSlingshotButton;
    public Button buildAmmoButton;
    public Button buildGliderButton;
    public Button buildGrappleHookButton;


    public float timeScale;

    private void Start()
    {
        currentBuildPanel = null;
        UpdateMaterialsCollected();
        gameUI.SetActive(true);
        buildPanelStatus = false;
        UpdateCanBuildText(false);
        buildPanel.SetActive(false);
    }
    private void Update()
    {
        //flashLightIntensity.text = "Light Power: " + 
            //FL.myLight.intensity.ToString("F2") + " /10";

        fallTimer.text = "Fall timer: " +  TPM.fallTimer.ToString("F2");

        currentOutfit.text = OM.outfits.ToString();

        ToggleBuildMenu();
    }

    #region Text Updaters

    public void UpdateMaterialsCollected()
    {
        UpdateRocksCollected();
        UpdateSticksCollected();
        UpdateMushroomsCollected();
        UpdatePebblesCollected();
    }
    public void UpdateRocksCollected()
    {
        smallRocksCollected.text = "Rocks Collected: " + GM.rocksCollected.ToString();
    }

    public void UpdateSticksCollected()
    {
        sticksCollected.text = "Sticks Collected: " + GM.sticksCollected.ToString();
    }

    public void UpdateMushroomsCollected()
    {
        mushroomsCollected.text = "Mushrooms Collected: " + GM.mushroomsCollected.ToString();
    }

    public void UpdatePebblesCollected()
    {
        pebblesCollected.text = "Pebbles Collected: " + GM.pebblesCollected.ToString();
    }

    public void UpdateCanBuildText(bool canBuild)
    {
        if (!canBuild)
        {
            this.canBuild.text = "";
        }
        if (canBuild)
        {
            this.canBuild.text = "Build";
        }
    }

    public void UpdateCanBuildText(string text)
    {
        canBuild.text = text;
    }
    #endregion

    #region Button Updaters

    public void IsButtonClickable()
    {
        if (!GM.havePickaxe)
        {
            if (BM.PickaxeCheck())
            {
                buildPickaxeButton.interactable = true;
            }
        }
        else
            buildPickaxeButton.interactable = false;

        if (!GM.haveSlingshot)
        {
            if (BM.SlingshotCheck())
            {
                buildSlingshotButton.interactable = true;
            }
        }
        else
            buildSlingshotButton.interactable = false;

        buildLadderButton.interactable = BM.LadderCheck();
        buildBridgeButton.interactable = BM.BridgeCheck();

        if (!GM.haveGrappleHook)
        {
            if (BM.GrappleHookCheck())
            {
                buildGrappleHookButton.interactable = true;
            }
        }
        else
            buildGrappleHookButton.interactable = false;

        if (!GM.haveGlider)
        {
            if (BM.GliderCheck())
            {
                buildGliderButton.interactable = true;
            }
        }
        else
            buildGliderButton.interactable = false;
    }
    #endregion
    public void ToggleBuildMenu()
    {
        if (IM.buildMenu_Input)
        {
            if(currentBuildPanel != null)
            {
                currentBuildPanel.SetActive(false);
            }
            

            switch (OM.outfits)
            {
                case Outfits.Miner:
                    currentBuildPanel = BuildPanels[0];
                    break;
                case Outfits.Builder:
                    currentBuildPanel = BuildPanels[1];
                    break;
                case Outfits.Slingshot:
                    currentBuildPanel = BuildPanels[2];
                    break;
                case Outfits.Grapple:
                    currentBuildPanel = BuildPanels[3];
                    break;
                case Outfits.Glider:
                    currentBuildPanel = BuildPanels[4];
                    break;
                case Outfits.Sailor:
                    currentBuildPanel = BuildPanels[5];
                    break;
            }
            IsButtonClickable();
            BuildMenuToggle();

            IM.buildMenu_Input = false;

        }
        else
        {
            IM.buildMenu_Input = false;
            return;
        }
    }
        


    public void BuildMenuToggle()
    {


        if (PC.paused)
            return;

        buildPanelStatus = !buildPanelStatus;
        buildPanel.SetActive(buildPanelStatus);
        if (buildPanelStatus)
        {
            currentBuildPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            OM.canChangeOutfits = false;

        }

        if (!buildPanelStatus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            OM.canChangeOutfits = true;

        }
    }
}
