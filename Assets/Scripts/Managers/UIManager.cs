using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum Menus { 
    None,
    Paused,
    Build,
    Radial
}


public class UIManager : GameBehaviour<UIManager>
{
    public Menus menu;

    [Header("Text")]
    public TMP_Text flashLightIntensity;
    public TMP_Text canBuild;
    public TMP_Text fallTimer;
    public TMP_Text smallRocksCollected;
    public TMP_Text sticksCollected;
    public TMP_Text mushroomsCollected;
    public TMP_Text pebblesCollected;
    public TMP_Text currentOutfit;
    public TMP_Text currentAmmoType;

    [Header("Control Text")]
    public TMP_Text outfitControlText1;
    public TMP_Text outfitControlText2;
    public TMP_Text outfitControlText3;
    public TMP_Text outfitControlText4;

    [Header("Radial Menu Text")]
    public TMP_Text minerText;
    public TMP_Text ladderText;
    public TMP_Text bridgeText;
    public TMP_Text utilityText;

    [Header("Panels")]
    public GameObject gameUI;
    public GameObject buildPanel;
    public GameObject SlingShotPanel;
    public GameObject RadialMenuPanel;
    public GameObject pausePanel;
    public GameObject CurrentPanel;
    public GameObject[] BuildPanels;
    [SerializeField]
    private GameObject currentBuildPanel;

    [Header("Bools")]
    public bool buildPanelStatus;
    public bool radialMenuStatus;
    public bool paused;

    [Header("Buttons")]
    public Button pickaxeButton;
    public Button buildLadderButton;
    public Button buildBridgeButton;
    //public Button buildSlingshotButton;
    public Button utilityButton;
    public Button buildAmmoButton;
    //public Button buildGliderButton;
    //public Button buildGrappleHookButton;


    public float timeScale;


    public Image LadderOutline;
    public Image BridgeOutline;


    private void Start()
    {
        // Set UI values for start of game
        currentBuildPanel = null;
        CurrentPanel = null;

        gameObject.SetActive(true);
        gameUI.SetActive(true);

        buildPanelStatus = false;    
        buildPanel.SetActive(false);
        RadialMenuPanel.SetActive(false);
        pausePanel.SetActive(false);
        paused = false;

        minerText.text = "";
        ladderText.text = "";
        bridgeText.text = "";
        utilityText.text = "";


        currentBuildPanel = BuildPanels[0];
        Time.timeScale = 1;

        UpdateCanBuildText(false);
        UpdateMaterialsCollected();
        UpdateControlText();

    }
    private void Update()
    {
        //flashLightIntensity.text = "Light Power: " + 
            //FL.myLight.intensity.ToString("F2") + " /10";

        fallTimer.text = "Fall timer: " +  TPM.fallTimer.ToString("F2");

        //currentOutfit.text = OM.outfit.ToString();

        Inputs();
        
    }

    #region Text Updaters

    public void UpdateToolsUnlockedUI(string tool)
    {
        if(tool == "Pickaxe")
        {
            minerText.text = "Pickaxe";
        }

        if(tool == "Builder")
        {
            ladderText.text = "Ladder";
            bridgeText.text = "Bridge";
        }

        if(tool == "GrappleHook" || tool == "Glider")
        {
            utilityText.text = "Utility";
        }

    }

    /// <summary>
    /// Update UI of materials player has collected
    /// </summary>
    public void UpdateMaterialsCollected()
    {
        UpdateRocksCollected();
        UpdateSticksCollected();
        UpdateMushroomsCollected();
        UpdatePebblesCollected();
    }

    /// <summary>
    /// Update UI of how many rocks the player has collected
    /// </summary>
    public void UpdateRocksCollected()
    {
        smallRocksCollected.text = "Rocks Collected: " + GM.rocksCollected.ToString();
    }

    /// <summary>
    /// Update the UI of how many sticks the player has collected
    /// </summary>
    public void UpdateSticksCollected()
    {
        sticksCollected.text = "Sticks Collected: " + GM.sticksCollected.ToString();
    }

    /// <summary>
    /// Update the UI of how many mushrooms the player has collected
    /// </summary>
    public void UpdateMushroomsCollected()
    {
        mushroomsCollected.text = "Mushrooms Collected: " + GM.mushroomsCollected.ToString();
    }

    /// <summary>
    /// Update the UI of how many Pebbles the player has collected
    /// </summary>
    public void UpdatePebblesCollected()
    {
        pebblesCollected.text = "Pebbles Collected: " + GM.pebblesCollected.ToString();
    }

    /// <summary>
    /// Update UI prompt for when the player can build
    /// </summary>
    /// <param name="canBuild">The bool that defines if the player can build </param>
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

    /// <summary>
    /// Tell the player if they can build, if not, tell the player what materials the player doesn't have enough of
    /// </summary>
    /// <param name="text">The string that defines the text to be displayed</param>
    public void UpdateBuildStatus(string text)
    {
        canBuild.text = text;
    }

    /// <summary>
    /// Reset the text for what controls the player has available to them
    /// </summary>
    private void ResetControlText()
    {
        outfitControlText1.text = "";
        outfitControlText2.text = "";
        outfitControlText3.text = "";
        outfitControlText4.text = "";
    }

    /// <summary>
    /// Update the text for what controls the player has avialable ot them according to what outfit the player currently is wearing
    /// </summary>
    public void UpdateControlText()
    {
        ResetControlText();
        switch (OM.outfit) 
        {
            case Outfits.Miner:
                outfitControlText1.text = "Mine: Right Click";
                break;
            case Outfits.Builder:
                outfitControlText1.text = "Open Build Menu: B";
                outfitControlText2.text = "Build object: Right Click";
                outfitControlText3.text = "Cancel Build: C";
                outfitControlText4.text = "Destroy Object: E";
                break;
            case Outfits.Slingshot:
                outfitControlText1.text = "Fire: Left Click";
                outfitControlText2.text = "Change Ammo Type: Scroll Wheel";

                break;
            case Outfits.Utility:
                outfitControlText1.text = "Glide: Hold Space";
                outfitControlText2.text = "Grapple Hook: Right Click";
                break;

        }
    }
    

    #endregion

    #region Button Updaters


    /// <summary>
    /// Check if UI buttons are clickable by running material checks
    /// </summary>
    public void IsButtonClickable()
    {
        pickaxeButton.interactable = GM.havePickaxe;

        //buildLadderButton.interactable = BM.LadderCheck() && GM.haveBuilding;
        //buildBridgeButton.interactable = BM.BridgeCheck(1) && GM.haveBuilding;

        if (GM.haveGlider || GM.haveGrappleHook)
            utilityButton.interactable = true;

        if (!GM.haveGlider && !GM.haveGrappleHook)
            utilityButton.interactable = false;

    }


    #endregion

    /// <summary>
    /// Manage UI inputs
    /// </summary>
    public void Inputs()
    {
        //Pause menu input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        if (paused)
            return;

        //if (IM.buildMenu_Input)
        //{
        //    if (menu != Menus.None)
        //    {
        //        if(menu != Menus.Build)
        //        {
        //            {
        //                IM.buildMenu_Input = false;
        //                return;
        //            }
        //        }
        //    }   
        //    menu = Menus.Build;
        //}

        //Radial menu input
        if (Input.GetKey(KeyCode.Tab))
        {
            if (menu != Menus.None)
            {
                if(menu != Menus.Radial)
                {
                    return;
                }
            }

            menu = Menus.Radial;
        }

        ToggleRadialMenu(radialMenuStatus);
        ToggleMenus();

    }

    /// <summary>
    /// Toggle UI depending on enum case
    /// </summary>
    public void ToggleMenus()
    {
        switch (menu)
        {
            default:
                //if (paused == true)
                //    Pause();

                Cursor.lockState = CursorLockMode.Locked;
                if(CurrentPanel != null)
                    CurrentPanel.SetActive(false);
        
                break;

            case Menus.Paused:
                ChangeMenu(pausePanel);
                Pause();
                break;

            case Menus.Build:
                if (IM.buildMenu_Input)
                {
                    ToggleBuildMenu();
                    ChangeMenu(currentBuildPanel);
                    
                }       
                break;

            case Menus.Radial:
                ChangeMenu(RadialMenuPanel);
                radialMenuStatus = true;
                if (Input.GetKey(KeyCode.Tab) == false)
                {
                    radialMenuStatus = false;
                    menu = Menus.None;
                }       
                break;
        }
    }

    /// <summary>
    /// Change which UI panel will be activated when this function is called
    /// </summary>
    /// <param name="menuToChangeTo">Panel to change to</param>
    private void ChangeMenu(GameObject menuToChangeTo)
    {
        if (menuToChangeTo == null)
            return;
 
        CurrentPanel = menuToChangeTo;
        CurrentPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void ToggleBuildMenu()
    {
        if (currentBuildPanel != null)
            {
                currentBuildPanel.SetActive(false);
            }


        switch (OM.outfit)
        {
            case Outfits.Miner:
                currentBuildPanel = null;
                
                break;
            case Outfits.Builder:
                currentBuildPanel = BuildPanels[0];
                break;
            case Outfits.Slingshot:
                currentBuildPanel = BuildPanels[1];
                break;
            case Outfits.Utility:
                currentBuildPanel = null;
                ResetControlText();
                break;


        }
        IM.buildMenu_Input = false;
        if (currentBuildPanel == null)
        {
            menu = Menus.None;
            return;
        }
            

        
        BuildMenuToggle();

        
    }
        
    
    public void BuildMenuToggle()
    {

        Debug.Log("Build Menu Toggled");

        buildPanelStatus = !buildPanelStatus;
        buildPanel.SetActive(buildPanelStatus);
        if (buildPanelStatus)
        {
            currentBuildPanel.SetActive(true);
            OM.canChangeOutfits = false;

        }

        if (!buildPanelStatus)
        {
            OM.canChangeOutfits = true;
            menu = Menus.None;

        }

        IM.buildMenu_Input = false;
    }

    public void Toggle(GameObject objectToToggle)
    {
        objectToToggle.SetActive(!objectToToggle);
    }

    /// <summary>
    /// Change the UI of current bullet type to the current bullet type that is active
    /// </summary>
    public void ChangeAmmoTypeText()
    {
        currentAmmoType.text = SS.currentBullet.name.ToString();
    }

    /// <summary>
    /// Change the active state of the radial menu depending on the bool passed in
    /// </summary>
    /// <param name="status"> bool that defines Radial menu active state</param>
    private void ToggleRadialMenu(bool status) 
    {
        RadialMenuPanel.SetActive(status);
        IsButtonClickable();
    }

    /// <summary>
    /// Pause the game and active the pause panel, manage cursor states
    /// </summary>
    public void Pause()
    {
        //Debug.Log("Paused");
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
        pausePanel.SetActive(paused);
        //Debug.Log(Time.timeScale);

        if (paused)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (!paused)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
