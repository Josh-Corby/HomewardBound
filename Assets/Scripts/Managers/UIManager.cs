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


    [Header("Panels")]
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

    [SerializeField]
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
        currentBuildPanel = BuildPanels[0];

        gameObject.SetActive(true);

        paused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1;

        UpdateControlText();

    }
    private void Update()
    {
        //flashLightIntensity.text = "Light Power: " + 
            //FL.myLight.intensity.ToString("F2") + " /10";

        fallTimer.text = "Fall timer: " +  TPM.fallTimer.ToString("F2");

        currentOutfit.text = OM.outfit.ToString();

        Inputs();
        
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

    private void ResetControlText()
    {
        outfitControlText1.text = "";
        outfitControlText2.text = "";
        outfitControlText3.text = "";
        outfitControlText4.text = "";
    }

    public void UpdateControlText()
    {
        ResetControlText();
        switch (OM.outfit) 
        {
            case Outfits.Miner:
                outfitControlText1.text = "Mine: Left Click";
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


    #endregion

    public void Inputs()
    {
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

        ToggleRadialMenu();
        ToggleMenus();

    }
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
            

        IsButtonClickable();
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
    public void ChangeAmmoTypeText()
    {

        currentAmmoType.text = SS.currentBullet.name.ToString();
    }

    private void ToggleRadialMenu() 
    {
        RadialMenuPanel.SetActive(radialMenuStatus);
    }

    public void Pause()
    {
        Debug.Log("Paused");
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
    }
}
