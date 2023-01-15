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
}
public class UIManager : GameBehaviour<UIManager>
{
    public Menus menu;

    [Header("Text")]

    public TMP_Text smallRocksCollected;
    public TMP_Text sticksCollected;
    public TMP_Text stringCollected;
    public TMP_Text pebblesCollected;

    [Header("Panels")]
    public GameObject gameUI;
    public GameObject pausePanel;
    private GameObject CurrentPanel;

    [Header("Bools")]
    public bool paused;

    [SerializeField]
    private Image[] HotbarOutlines;
    private Image CurrentOutline;

    public GameObject DialoguePanel;
    public TMP_Text currentNPC_Name_Text;
    public TMP_Text current_NPC_Dialogue_Text;

    [SerializeField]
    private Color outlineColour;


    [SerializeField]
    private GameObject _settingsPanel;
    [SerializeField]
    private Slider _sensitivitySlider;

    private CameraTransform cam;


    [SerializeField]
    private GameObject _ladderImage;
    [SerializeField]
    private GameObject _bridgeImage;
    [SerializeField]
    private GameObject _slingshotImage;

    public GameObject creditsUI;

    public bool creditsOn;
    private void Awake()
    {
        cam = TPM.gameObject.GetComponentInChildren<CameraTransform>();
    }
    private void Start()
    {
        
        _sensitivitySlider.value = cam.Sensitivity;
        // Set UI values for start of game
        gameUI.SetActive(true);
        pausePanel.SetActive(false);
        paused = false;
        Time.timeScale = 1;
        UpdateMaterialsCollected();
        SetSlingshotUIStatus(false);
        SetBuildUIStatus(false);

    }
    private void Update()
    {
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
        UpdateStringCollected();
        //UpdatePebblesCollected();
    }
    public void UpdateMaterials(TMP_Text text, int amount)
    {
        text.text = amount.ToString();
    }
    /// <summary>
    /// Update UI of how many rocks the player has collected
    /// </summary>
    public void UpdateRocksCollected()
    {
        smallRocksCollected.text = GM.RocksCollected.ToString();
    }
    /// <summary>
    /// Update the UI of how many sticks the player has collected
    /// </summary>
    public void UpdateSticksCollected()
    {
        sticksCollected.text = GM.SticksCollected.ToString();
    }
    /// <summary>
    /// Update the UI of how many mushrooms the player has collected
    /// </summary>
    public void UpdateStringCollected()
    {
        stringCollected.text = GM.StringCollected.ToString();
    }
    /// <summary>
    /// Update the UI of how many Pebbles the player has collected
    /// </summary>
    public void UpdatePebblesCollected()
    {
        //pebblesCollected.text = GM.PebblesCollected.ToString();
    }
    /// <summary>
    /// Update UI prompt for when the player can build
    /// </summary>
    /// <param name="canBuild">The bool that defines if the player can build </param>

    /// <summary>
    /// Tell the player if they can build, if not, tell the player what materials the player doesn't have enough of
    /// </summary>
    /// <param name="text">The string that defines the text to be displayed</param>
    #endregion

    /// <summary>
    /// Manage UI inputs
    /// </summary>
    public void Inputs()
    {
        if (creditsOn)
        {
            return;
        }
        //Pause menu input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        if (paused)
        {
            return;
        }
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
            _settingsPanel.SetActive(false);
        }
    }

    public void ToggleSettings()
    {
        _settingsPanel.SetActive(!_settingsPanel.activeSelf);
        pausePanel.SetActive(!pausePanel.activeSelf);
    }

    public void SelectControlUI(int panelIndex)
    {
        Debug.Log(panelIndex);
        SelectHotbarOutline(HotbarOutlines[panelIndex]);
    }

    private void SelectHotbarOutline(Image outline)
    {
        if (CurrentOutline != null)
        {
            DeselectHotbarOutline();
        }
        CurrentOutline = outline;
        CurrentOutline.color = outlineColour;
    }

    public void DeselectHotbarOutline()
    {
        if (CurrentOutline != null)
        {

            CurrentOutline.color = Color.white;
            CurrentOutline = null;
        }
    }

    public void SetSensitivity(float sensitivity)
    {
        _sensitivitySlider.value = sensitivity;
    }
    public void ChangeSensitivity(Slider slider)
    {
        cam.ChangeSensitivity(slider.value);
    }

    public void SetBuildUIStatus(bool status)
    {
        _ladderImage.SetActive(status);
        _bridgeImage.SetActive(status);
    }

    public void SetSlingshotUIStatus(bool status)
    {
        _slingshotImage.SetActive(status);
    }

    public void DisableCredits()
    {
        creditsOn = false;
        creditsUI.SetActive(false);
        gameUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}
