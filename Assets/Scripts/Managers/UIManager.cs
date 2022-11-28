using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public enum Menus
{
    None,
    Paused,
    Build,
    Radial
}
public class UIManager : GameBehaviour<UIManager>
{
    public Menus menu;

    [Header("Text")]
    public TMP_Text canBuild;
    public TMP_Text fallTimer;
    public TMP_Text smallRocksCollected;
    public TMP_Text sticksCollected;
    public TMP_Text mushroomsCollected;
    public TMP_Text pebblesCollected;
    public TMP_Text currentOutfit;
    public TMP_Text currentAmmoType;

    [Header("Panels")]
    public GameObject gameUI;

    public GameObject pausePanel;
    public GameObject CurrentPanel;

    [Header("Bools")]

    public bool paused;

    public Image LadderOutline;
    public Image BridgeOutline;
    public Image BonfireOutline;

    [SerializeField]
    private GameObject currentMaterialPanel;
    [SerializeField]
    private int currentMaterialPanelIndex;
    [SerializeField]
    private GameObject[] materialUIPanels;

    private TMP_Text[] materialCosts;

    [SerializeField]
    private Image[] HotbarOutlines;

    private Image CurrentOutline;

    [SerializeField]
    private GameObject TestUI;
    [SerializeField]
    private GameObject[] TestUIPanels;
    private GameObject currentTestPanel;
    private int testPanelIndex;

    [SerializeField]
    private GameObject checkpointText;
    private void Start()
    {
        PlayerManager.OnToolSelected += SelectControlUI;
        // Set UI values for start of game
        //currentBuildPanel = null;
        //CurrentPanel = null;
        gameObject.SetActive(true);
        gameUI.SetActive(true);
        //buildPanelStatus = false;    
        //buildPanel.SetActive(false);
        //RadialMenuPanel.SetActive(false);
        pausePanel.SetActive(false);
        paused = false;
        //minerText.text = "";
        //ladderText.text = "";
        //bridgeText.text = "";
        //utilityText.text = "";
        //currentBuildPanel = BuildPanels[0];
        Time.timeScale = 1;
        //UpdateCanBuildText(false);
        UpdateMaterialsCollected();
        //UpdateControlText();
        TestUI.SetActive(false);
        GameStartUI();
        checkpointText.SetActive(false);
    }
    private void Update()
    {
        //flashLightIntensity.text = "Light Power: " + 
        //FL.myLight.intensity.ToString("F2") + " /10";
        fallTimer.text = "Fall timer: " + TPM.fallTimer.ToString("F2");
        //currentOutfit.text = OM.outfit.ToString();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeTestUI();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleTestUI();
        }
        Inputs();
    }
    private void ToggleTestUI()
    {
        TestUI.SetActive(!TestUI.activeSelf);
        gameUI.SetActive(!gameUI.activeSelf);
    }
    private void GameStartUI()
    {
        for (int i = 0; i < TestUIPanels.Length; i++)
        {
            TestUIPanels[i].SetActive(false);
        }
        TestUIPanels[0].SetActive(true);
        currentTestPanel = TestUIPanels[0];
        testPanelIndex = 0;
    }
    private void ChangeTestUI()
    {
        if (!TestUI.activeSelf)
        {
            return;
        }

        currentTestPanel.SetActive(false);
        if (testPanelIndex == TestUIPanels.Length - 1)
        {
            testPanelIndex = 0;
        }
        else
        {
            testPanelIndex += 1;
        }

        TestUIPanels[testPanelIndex].SetActive(true);
        currentTestPanel = TestUIPanels[testPanelIndex];
    }
    #region Text Updaters


    /// <summary>
    /// Update UI of materials player has collected
    /// </summary>
    public void UpdateMaterialsCollected()
    {
        UpdateRocksCollected();
        UpdateSticksCollected();
        UpdateMushroomsCollected();
        //UpdatePebblesCollected();
    }
    public void UpdateMaterials(TMP_Text text, string material, int amount)
    {
        text.text =  amount.ToString();
    }
    /// <summary>
    /// Update UI of how many rocks the player has collected
    /// </summary>
    public void UpdateRocksCollected()
    {
        smallRocksCollected.text = GM.rocksCollected.ToString();
    }
    /// <summary>
    /// Update the UI of how many sticks the player has collected
    /// </summary>
    public void UpdateSticksCollected()
    {
        sticksCollected.text = GM.sticksCollected.ToString();
    }
    /// <summary>
    /// Update the UI of how many mushrooms the player has collected
    /// </summary>
    public void UpdateMushroomsCollected()
    {
        mushroomsCollected.text = GM.mushroomsCollected.ToString();
    }
    /// <summary>
    /// Update the UI of how many Pebbles the player has collected
    /// </summary>
    public void UpdatePebblesCollected()
    {
        pebblesCollected.text = GM.pebblesCollected.ToString();
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
    ///// </summary>
    //private void ResetControlText()
    //{
    //    outfitControlText1.text = "";
    //    outfitControlText2.text = "";
    //    outfitControlText3.text = "";
    //    outfitControlText4.text = "";
    //}

    /// <summary>
    /// Update the text for what controls the player has avialable ot them according to what outfit the player currently is wearing
    ///// </summary>
    //public void UpdateControlText()
    //{
    //    ResetControlText();
    //    switch (OM.outfit)
    //    {
    //        case Outfits.Miner:
    //            outfitControlText1.text = "Mine: Right Click";
    //            break;
    //        case Outfits.Builder:
    //            outfitControlText1.text = "Open Build Menu: B";
    //            outfitControlText2.text = "Build object: Right Click";
    //            outfitControlText3.text = "Cancel Build: C";
    //            outfitControlText4.text = "Destroy Object: E";
    //            break;
    //        case Outfits.Slingshot:
    //            outfitControlText1.text = "Fire: Left Click";
    //            outfitControlText2.text = "Change Ammo Type: Scroll Wheel";

    //            break;
    //        case Outfits.Utility:
    //            outfitControlText1.text = "Glide: Hold Space";
    //            outfitControlText2.text = "Grapple Hook: Right Click";
    //            break;

    //    }
    //}


    #endregion

    #region Button Updaters


    /// <summary>
    /// Check if UI buttons are clickable by running material checks
    /// </summary>
    //public void IsButtonClickable()
    //{
    //    pickaxeButton.interactable = GM.havePickaxe;

    //    //buildLadderButton.interactable = BM.LadderCheck() && GM.haveBuilding;
    //    //buildBridgeButton.interactable = BM.BridgeCheck(1) && GM.haveBuilding;

    //    if ( GM.haveGrappleHook)
    //        utilityButton.interactable = true;

    //    if (!GM.haveGrappleHook)
    //        utilityButton.interactable = false;

    //}


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

        #region old code
        /*
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
        //if (Input.GetKey(KeyCode.Tab))
        //{
        //    if (menu != Menus.None)
        //    {
        //        if(menu != Menus.Radial)
        //        {
        //            return;
        //        }
        //    }

        //    menu = Menus.Radial;
        //}
        */
        #endregion
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
                if (CurrentPanel != null)
                    CurrentPanel.SetActive(false);
                break;

            case Menus.Paused:
                ChangeMenu(pausePanel);
                Pause();
                break;

                //case Menus.Build:
                //    if (IM.buildMenu_Input)
                //    {
                //        ToggleBuildMenu();
                //        ChangeMenu(currentBuildPanel);

                //    }
                //    break;
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

    //public void ToggleBuildMenu()
    //{
    //    if (currentBuildPanel != null)
    //    {
    //        currentBuildPanel.SetActive(false);
    //        { 

    //    switch (OM.outfit)
    //    {
    //        case Outfits.Miner:
    //            currentBuildPanel = null;

    //            break;
    //        case Outfits.Builder:
    //            currentBuildPanel = BuildPanels[0];
    //            break;
    //        case Outfits.Slingshot:
    //            currentBuildPanel = BuildPanels[1];
    //            break;
    //        case Outfits.Utility:
    //            currentBuildPanel = null;
    //            ResetControlText();
    //            break;


    //    }
    //    IM.buildMenu_Input = false;
    //    if (currentBuildPanel == null)
    //    {
    //        menu = Menus.None;
    //        return;
    //    }

    //    BuildMenuToggle();
    //}

    //public void BuildMenuToggle()
    //{

    //    Debug.Log("Build Menu Toggled");

    //    buildPanelStatus = !buildPanelStatus;
    //    buildPanel.SetActive(buildPanelStatus);
    //    if (buildPanelStatus)
    //    {
    //        currentBuildPanel.SetActive(true);
    //        OM.canChangeOutfits = false;

    //    }

    //    if (!buildPanelStatus)
    //    {
    //        OM.canChangeOutfits = true;
    //        menu = Menus.None;

    //    }

    //    IM.buildMenu_Input = false;
    //}

    public void Toggle(GameObject objectToToggle)
    {
        objectToToggle.SetActive(!objectToToggle);
    }

    /// <summary>
    /// Change the UI of current bullet type to the current bullet type that is active
    /// </summary>
    //public void ChangeAmmoTypeText()
    //{
    //    currentAmmoType.text = SS.currentBullet.name.ToString();
    //}

    /// <summary>
    /// Change the active state of the radial menu depending on the bool passed in
    /// </summary>
    /// <param name="status"> bool that defines Radial menu active state</param>
    //private void ToggleRadialMenu(bool status)
    //{
    //    RadialMenuPanel.SetActive(status);
    //    IsButtonClickable();
    //}

    /// <summary>
    /// Pause the game and active the pause panel, manage cursor states
    /// </summary>
    public void Pause()
    {
        //Debug.Log("Paused");
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;

        pausePanel.SetActive(paused);
        gameUI.SetActive(!paused);
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

    public IEnumerator CheckpointPopup()
    {
        checkpointText.SetActive(true);
        Debug.Log("bonfire found");
        checkpointText.GetComponent<Animator>().Play("CheckpointText");
        yield return new WaitForSeconds(1.5f);
        checkpointText.SetActive(false);
    }
    public void SelectMaterialUI(int panelIndex)
    {


        //if (currentMaterialPanel != null)
        //{
        //    currentMaterialPanel.SetActive(false);
        //}

        //if (panelIndex >= 1 && panelIndex <= 2)
        //{
        //    currentMaterialPanelIndex = 1;
        //}

        //if (panelIndex >= 3)
        //{
        //    currentMaterialPanelIndex = panelIndex - 1;

        //}
        //currentMaterialPanel = materialUIPanels[currentMaterialPanelIndex - 1];
        //currentMaterialPanel.SetActive(true);
        //Debug.Log("panel selected");
    }

    public void DisablePanel()
    {
        //Debug.Log(currentMaterialPanel);
        currentMaterialPanel.SetActive(false);
        currentMaterialPanel = null;
    }
    private void SelectControlUI(int panelIndex)
    {
        SelectHotbarOutline(HotbarOutlines[panelIndex - 1]);
    }

    private void SelectHotbarOutline(Image outline)
    {
        if (CurrentOutline != null)
        {
            DeselectHotbarOutline();
        }

        CurrentOutline = outline;
        CurrentOutline.color = Color.red;
    }
    public void DeselectHotbarOutline()
    {

        CurrentOutline.color = Color.black;
        CurrentOutline = null;
    }

}
