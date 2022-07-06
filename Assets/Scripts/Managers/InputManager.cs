using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : GameBehaviour<InputManager>
{
    PlayerControls playerControls;

    public Vector2 movementInput, cameraInput;
    public float cameraInputX, cameraInputY;
    public float moveAmount;
    public float verticalInput, horizontalInput;
    public bool sprint_Input;
    public bool jump_Input;
    public bool interact_Input;
    public bool glide_Input;


    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.Sprint.performed += i => sprint_Input = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprint_Input = false;

            playerControls.PlayerActions.Jump.performed += i => jump_Input = true;

            playerControls.PlayerActions.Interact.performed += i => interact_Input = true;

            playerControls.PlayerActions.Glide.performed += i => glide_Input = true;
            playerControls.PlayerActions.Glide.canceled += i => glide_Input = false;
        }

        playerControls.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        AM.UpdateAnimatorValues(0, moveAmount, PL.isSprinting);
    }

    private void HandleSprintingInput()
    {
        if (sprint_Input && moveAmount > 0.5f)
        {
            PL.isSprinting = true;
        }
        else
        {
            PL.isSprinting = false;
        }
    }

    private void HandleJumpingInput()
    {
        if (jump_Input)
        {
            jump_Input = false;
            PL.HandleJumping();
        }
    }

}
