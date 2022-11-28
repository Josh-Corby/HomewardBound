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

        gameObject.SetActive(true);
        gameUI.SetActive(true);
        pausePanel.SetActive(false);
        paused = false;
        Time.timeScale = 1;
        UpdateMaterialsCollected();
        TestUI.SetActive(false);
        checkpointText.SetActive(false);
    }
    private void Update()
    {
        fallTimer.text = "Fall timer: " + TPM.fallTimer.ToString("F2");
        Inputs();
       
        #region Text Updaters


        /// <summary>
        /// Update UI of materials player has collected
        /// </summary>
        /// 
    }
    public void UpdateMaterialsCollected()
    {
        UpdateRocksCollected();
        UpdateSticksCollected();
        UpdateMushroomsCollected();
        //UpdatePebblesCollected();
    }
    public void UpdateMaterials(TMP_Text text, int amount)
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

                Cursor.lockState = CursorLockMode.Locked;
                if (CurrentPanel != null)
                    CurrentPanel.SetActive(false);
                break;

            case Menus.Paused:
                ChangeMenu(pausePanel);
                Pause();
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

    public void Toggle(GameObject objectToToggle)
    {
        objectToToggle.SetActive(!objectToToggle);
    }
 
    /// <summary>
    /// Pause the game and active the pause panel, manage cursor states
    /// </summary>
    public void Pause()
    {
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
        pausePanel.SetActive(paused);
        gameUI.SetActive(!paused);

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
