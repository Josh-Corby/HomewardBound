using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : GameBehaviour<PlayerLocomotion>
{
    Rigidbody playerRigidBody;

    public Vector3 moveDirection;
    Transform cameraObject;

    [Header("Falling")]
    public float inAirTimer;

    public float fallingVelocity;
    public float glideVelocity;

    public float fallTimer;
    public float fallTimerMax = 3f;


    [Header("Movement Speeds")]
    public float mudSpeed = 0.5f;
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5f;
    public float sprintingSpeed = 7f;
    public float climbSpeed = 5f;

    /*

    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
        fallTimer = fallTimerMax;
    }

    public void HandleAllMovement()
    {
         if (!IM.glide_Input)
                //isGliding = false;
        if (PM.isClimbing)
        {
            HandleClimbing();
            return;
        }
        

        HandleFallingAndLanding();
        if (PM.isInteracting)
        {
            return;
        }
    }

    private void HandleMovement()
    {
      
        if (isSprinting)
        {
            moveDirection *= sprintingSpeed;
        }
        else
        {
            if (IM.moveAmount >= 0.5f)
            {
                moveDirection *= runningSpeed;
            }
            else
            {
                moveDirection *= walkingSpeed;
            }
        }

        Vector3 movementVelocity = moveDirection;
        playerRigidBody.velocity = movementVelocity;
    }
  
    private void HandleFallingAndLanding()
    {
       

        if (BM.haveGlider)
        {
            //Glide check
            if (IM.glide_Input)
            {
                //isGliding = true;
                Debug.Log("Player is gliding");
            }    
        }
        {
            fallTimer = fallTimerMax;
            inAirTimer = 0f;
            inAirTimer += Time.deltaTime;


        }
       

        //Grounded/landing check
       
        if //(isGrounded)
        {
            //isGliding = false;
            //inAirTimer = 0;
        }
           

        //Ground movement
        
    }


    public void HandleClimbing()
    {
        moveDirection = new Vector3(0, 0, 0);
        moveDirection.y = IM.verticalInput;
        moveDirection *= climbSpeed;
        Vector3 movementVelocity = moveDirection;
        playerRigidBody.velocity = movementVelocity;
    }
    */
}
