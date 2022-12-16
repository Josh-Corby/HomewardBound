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
    public bool CancelInput;
    public bool LeftClickInput;


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

            playerControls.PlayerActions.Cancel.performed += i => CancelInput = true;
            playerControls.PlayerActions.LeftClick.performed += i => LeftClickInput = true;
            playerControls.PlayerActions.LeftClick.canceled += i => LeftClickInput = false;

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
