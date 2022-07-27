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

    public TMP_Text flashLightIntensity;
    public TMP_Text canBuild;
    public TMP_Text fallTimer;
    public TMP_Text smallRocksCollected;
    public TMP_Text sticksCollected;
    public TMP_Text mushroomsCollected;
    public TMP_Text pebblesCollected;
    public TMP_Text currentOutfit;
    public TMP_Text currentAmmoType;

    public GameObject gameUI;
    public GameObject buildPanel;
    public GameObject SlingShotPanel;
    public GameObject RadialMenuPanel;
    public GameObject pausePanel;
    public GameObject CurrentPanel;
    

    public bool buildPanelStatus;
    public bool radialMenuStatus;
    public bool paused;


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
        RadialMenuPanel.SetActive(false);
        CurrentPanel = null;

        gameObject.SetActive(true);

        paused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1;

    }
    private void Update()
    {
        //flashLightIntensity.text = "Light Power: " + 
            //FL.myLight.intensity.ToString("F2") + " /10";

        fallTimer.text = "Fall timer: " +  TPM.fallTimer.ToString("F2");

        currentOutfit.text = OM.outfit.ToString();

        ToggleRadialMenu();

        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();

        BuildMenuInput();

        ToggleMenus();
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
        /*
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

        */
        buildLadderButton.interactable = BM.LadderCheck();
        buildBridgeButton.interactable = BM.BridgeCheck();
        
        /*
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
        */
    }

    public void BuildMenuInput()
    {
        if (IM.buildMenu_Input)
        {
            menu = Menus.Build;
        }
    }
    #endregion

    public void ToggleMenus()
    {
        switch (menu)
        {
            case Menus.None:
                CurrentPanel.SetActive(false);
                CurrentPanel = null;
                break;
            case Menus.Paused:
                break;
            case Menus.Build:
                ToggleBuildMenu();
                buildPanel.SetActive(true);
                CurrentPanel = currentBuildPanel;
                CurrentPanel.SetActive(true);
                break;
            case Menus.Radial:
                break;
        }


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
                    currentBuildPanel = BuildPanels[0];
                    break;
                case Outfits.Builder:
                    currentBuildPanel = BuildPanels[1];
                    break;
                case Outfits.Slingshot:
                    currentBuildPanel = BuildPanels[2];
                    break;
                case Outfits.Utility:
                    currentBuildPanel = BuildPanels[3];
                    break;


            }
            IsButtonClickable();
            //BuildMenuToggle();

            IM.buildMenu_Input = false;
    }
        
    public void BuildMenuToggle()
    {

        

        //buildPanelStatus = !buildPanelStatus;
        //buildPanel.SetActive(buildPanelStatus);
        //if (buildPanelStatus)
        //{
        //    currentBuildPanel.SetActive(true);
        //    Cursor.lockState = CursorLockMode.None;
        //    OM.canChangeOutfits = false;

        //}

        //if (!buildPanelStatus)
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //    OM.canChangeOutfits = true;

        //}
    }

    public void Toggle(GameObject objectToToggle)
    {
        objectToToggle.SetActive(!objectToToggle);
    }
    public void ChangeAmmoTypeText()
    {

        currentAmmoType.text = SS.currentBullet.name.ToString();
    }

    private void ToggleRadialMenu() 
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            radialMenuStatus = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            radialMenuStatus = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        RadialMenuPanel.SetActive(radialMenuStatus);

    }

    public void Pause()
    {
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
        pausePanel.SetActive(paused);

        if (paused)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (!paused)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (!paused && UI.buildPanelStatus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
