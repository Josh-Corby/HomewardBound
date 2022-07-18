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

    public GameObject gameUI;
    public GameObject buildPanel;
    public bool buildPanelStatus;

    public Button buildLadderButton;
    public Button buildBridgeButton;
    public Button buildGliderButton;
    public Button buildGrappleHookButton;


    

    public float timeScale;

    private void Start()
    {
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

        //fallTimer.text = "Fall timer: " +  PL.fallTimer.ToString("F2");

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
        buildLadderButton.interactable = BM.LadderCheck();
        buildBridgeButton.interactable = BM.BridgeCheck();
        buildGliderButton.interactable = BM.GliderCheck();
        buildGrappleHookButton.interactable = BM.GrappleHookCheck();
    }
    #endregion
    public void ToggleBuildMenu()
    {
        
        if (BM.haveGlider)
        {
            buildGliderButton.interactable = false;
        }
        if (BM.haveGrappleHook)
        {
            buildGrappleHookButton.interactable = false;
        }



        if (IM.buildMenu_Input)
        {
            IsButtonClickable();
            BuildMenuToggle();
            
            
            IM.buildMenu_Input = false;
        
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
            Cursor.lockState = CursorLockMode.None;

        }

        if (!buildPanelStatus)
        {
            Cursor.lockState = CursorLockMode.Locked;

        }
    }
}
