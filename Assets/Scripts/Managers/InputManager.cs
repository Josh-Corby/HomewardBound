using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : GameBehaviour<InputManager>
{
    PlayerControls playerControls;

    [Header("Definitions of all inputs and what type they are")]
    public Vector2 MovementInput, cameraInput;
    public float moveAmount;
    public float verticalInput, horizontalInput;
    public float mouseScrollY;
    public bool sprint_Input;
    public bool jump_Input;
    public bool interact_Input;
    public bool glide_Input;
    public bool buildMenu_Input;
    public bool destroy_Input;
    public bool cancel_Input;
    public bool lClick_Input;
    public bool rClick_Input;



        /// <summary>
        /// Define all inputs and how the player performs them
        /// /// </summary>
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => MovementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.Sprint.performed += i => sprint_Input = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprint_Input = false;

            playerControls.PlayerActions.Jump.performed += i => jump_Input = true;
            playerControls.PlayerActions.Jump.canceled += i => jump_Input = false;

            playerControls.PlayerActions.Interact.performed += i => interact_Input = true;

            playerControls.PlayerActions.Glide.performed += i => glide_Input = true;
            playerControls.PlayerActions.Glide.canceled += i => glide_Input = false;

            playerControls.PlayerActions.OpenBuildMenu.performed += i => buildMenu_Input = true;

            playerControls.PlayerActions.Destroy.performed += i => destroy_Input = true;

            playerControls.PlayerActions.Cancel.performed += i => cancel_Input = true;

            playerControls.PlayerActions.LeftClick.performed += i => lClick_Input = true;
            playerControls.PlayerActions.LeftClick.canceled += i => lClick_Input = false;

            playerControls.PlayerActions.RightClick.performed += i => rClick_Input = true;
            playerControls.PlayerActions.RightClick.canceled += i => rClick_Input = false;

            playerControls.PlayerActions.Scroll.performed += i => mouseScrollY = i.ReadValue<float>();
        }

        playerControls.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Disable Player Inputs
    /// </summary>
    private void OnDisable()
    {
        playerControls.Disable();
    }
}
