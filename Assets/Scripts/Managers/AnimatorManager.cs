using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : GameBehaviour<AnimatorManager>
{
    private Animator animator;

    private const string IS_WALKING = "isWalking";
    private const string IS_JUMPING = "isJumping";
    private const string IS_GROUNDED = "isGrounded";
    private const string IS_WALKING_BACK = "isWalkingBack";
    private const string IS_STEPPING_LEFT = "isSteppingLeft";
    private const string IS_STEPPING_RIGHT = "isSteppingRight";
    private const string IS_SPRINTING = "isSprinting";
    private const string IS_TURNING_LEFT = "isTurningLeft";
    private const string IS_TURNING_RIGHT = "isTurningRight";
    private const string IS_CLIMBING = "isClimbing";
    private const string IS_ON_LADDER = "isOnLadder";


    private bool isJumping;
    public bool isTurningLeft;
    public bool isTurningRight;
    public bool isClimbing;
    public bool isOnLadder;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        LadderClimb.OnLadderStateChange += SetOnLadder;
        LadderClimb.OnClimbingStateChange += SetClimbing;
        ThirdPlayerMovement.OnSprintingStateChange += SetSprinting;
        ThirdPlayerMovement.OnGroundedStateChange += SetGrounded;
        ThirdPlayerMovement.OnJump += SetJumping;
    }

    private void OnDisable()
    {
        LadderClimb.OnLadderStateChange -= SetOnLadder;
        LadderClimb.OnClimbingStateChange -= SetClimbing;
        ThirdPlayerMovement.OnSprintingStateChange -= SetSprinting;
        ThirdPlayerMovement.OnGroundedStateChange -= SetGrounded;
        ThirdPlayerMovement.OnJump -= SetJumping;
    }

    private void Update()
    {
        ManageAnimations();
    }
    private void ManageAnimations()
    {
        if (isOnLadder)
        {
            {
                SetOnLadder(true);
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                SetClimbing(true);
                return;
            }
            else
            {
                SetClimbing(false);
                return;
            }
        }

        else if (isJumping == true)
        {
            return;
        }

        else if (isOnLadder == false)
        {
            isTurningLeft = IM.cameraInput.x < 0;
            isTurningRight = IM.cameraInput.x > 0;

            SetTurnLeft(isTurningLeft);
            SetTurnRight(isTurningRight);

            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
            {
                SetWalking(false);
                SetWalkingBack(false);
                return;
            }

            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
            {
                SetStepLeft(false);
                SetStepRight(false);
                return;
            }

            if (Input.GetKey(KeyCode.A))
            {
                SetStepLeft(true);
                return;
            }

            if (Input.GetKey(KeyCode.W))
            {
                SetWalking(true);
                return;
            }

            if (Input.GetKey(KeyCode.S))
            {
                SetWalkingBack(true);
                return;
            }
            if (Input.GetKey(KeyCode.A))
            {
                SetStepLeft(true);
                return;
            }

            if (Input.GetKey(KeyCode.D))
            {
                SetStepRight(true);
                return;
            }

            else
            {
                SetWalking(false);
                SetWalkingBack(false);
                SetStepLeft(false);
                SetStepRight(false);

            } 
        }      
    }

    private void SetWalking(bool value)
    {
        animator.SetBool(IS_WALKING, value);
    }
    private void SetJumping(bool value)
    {
        isJumping = value;
        animator.SetBool(IS_JUMPING, value);
    }
    private void SetWalkingBack(bool value)
    {
        animator.SetBool(IS_WALKING_BACK, value);
    }
    private void SetStepLeft(bool value)
    {
        animator.SetBool(IS_STEPPING_LEFT, value);
    }
    private void SetStepRight(bool value)
    {
        animator.SetBool(IS_STEPPING_RIGHT, value);
    }
    private void SetGrounded(bool value)
    {
        animator.SetBool(IS_GROUNDED, value);
    }
    private void SetSprinting(bool value)
    {
        animator.SetBool(IS_SPRINTING, value);
    }
    private void SetTurnLeft(bool value)
    {
        animator.SetBool(IS_TURNING_LEFT, value);
    }
    private void SetTurnRight(bool value)
    {
        animator.SetBool(IS_TURNING_RIGHT, value);
    }
    private void SetClimbing(bool value)
    {
        if (isOnLadder)
        {
            if (value)
            {
                animator.SetBool(IS_CLIMBING, value);
                animator.enabled = true;
            }
            if (!value)
            {
                animator.enabled = false;
            }
        }
        else
        {
            animator.enabled = true;
        }
    }
    public void SetOnLadder(bool value)
    {
        Debug.Log(value);
        animator.SetBool(IS_ON_LADDER, value);
        isOnLadder = value;
    }

}
