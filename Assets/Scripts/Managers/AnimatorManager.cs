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


    private bool _isWalking;
    private bool _isRunning;
    private bool _isWalkingBack;
    private bool _isJumping;
    public bool IsTurningLeft;
    public bool IsTurningRight;
    private bool _isSteppingLeft;
    private bool _isSteppingRight;

    public bool IsClimbing;
    public bool IsOnLadder;
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
        if (IsOnLadder)
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

        else if (_isJumping == true)
        {
            return;
        }

        else if (IsOnLadder == false)
        {
            IsTurningLeft = IM.cameraInput.x < 0;
            IsTurningRight = IM.cameraInput.x > 0;

            SetTurnLeft(IsTurningLeft);
            SetTurnRight(IsTurningRight);

            _isWalking = Input.GetKey(KeyCode.W);
            _isSteppingLeft = Input.GetKey(KeyCode.A);
            _isSteppingRight = Input.GetKey(KeyCode.D);
            _isWalkingBack = Input.GetKey(KeyCode.S);

            if (_isWalking ^ _isWalkingBack)
            {
                SetWalking(_isWalking);
                SetWalkingBack(_isWalkingBack);
            }
            else
            {
                SetWalking(false);
                SetWalkingBack(false);
            }

            if (_isSteppingLeft ^ _isSteppingRight)
            {
                SetStepLeft(_isSteppingLeft);
                SetStepRight(_isSteppingRight);
            }
            else
            {
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
        _isJumping = value;
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
    public void SetClimbing(bool value)
    {
        if (IsOnLadder)
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
        IsOnLadder = value;
        animator.SetBool(IS_ON_LADDER, value);

        if (value == false)
        {
            SetClimbing(false);
        }
    }

}
