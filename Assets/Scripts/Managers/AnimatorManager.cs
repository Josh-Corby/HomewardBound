using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : GameBehaviour<AnimatorManager>
{
    private Animator animator;

    private bool isWalking;
    private bool isJumping;
    private bool isGrounded;
    private bool isWalkingBack;

    private bool isSteppingLeft;
    private bool isSteppingRight;
    public bool isSprinting;


    public bool isTurningLeft;
    public bool isTurningRight;
    public bool isClimbing;
    public bool isOnLadder;
    private void Awake()
    {
        animator = GetComponent<Animator>();

    }
    private void Start()
    {
        isSprinting = false;
    }

    private void Update()
    {
        ManageAnimations();
    }
    private void ManageAnimations()
    {
        if (TPM.groundState == GroundStates.Grounded)
        {
            SetAnimationBool("isGrounded", true);
        }

        if (TPM.groundState != GroundStates.Grounded)
        {
            SetAnimationBool("isGrounded", false);
        }
        if (isJumping == true) return;



        if (!isOnLadder)
        {
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
            {
                SetAnimationBool("isWalkingBack", isWalkingBack = false);
                SetAnimationBool("isWalking", isWalking = false);
                return;
            }

            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
            {
                SetAnimationBool("isSteppingLeft", isSteppingLeft = false);
                SetAnimationBool("isSteppingRight", isSteppingRight = false);
                return;
            }

            isSteppingLeft = Input.GetKey(KeyCode.A);
            SetAnimationBool("isSteppingLeft", isSteppingLeft);

            isSteppingRight = Input.GetKey(KeyCode.D);
            SetAnimationBool("isSteppingRight", isSteppingRight);

            SetAnimationBool("isWalking", isWalking = Input.GetKey(KeyCode.W));
            SetAnimationBool("isWalkingBack", isWalkingBack = Input.GetKey(KeyCode.S));


            isTurningLeft = IM.cameraInput.x < 0;
            isTurningRight = IM.cameraInput.x > 0;

            SetAnimationBool("isTurningLeft", isTurningLeft);
        }
        SetAnimationBool("isTurningRight", isTurningRight);
    }

    public void Jump()
    {
        SetAnimationBool("isJumping", true);

    }

    public void StopJumping()
    {
        SetAnimationBool("isJumping", isJumping);
    }

    public void SetBool(bool boolToChange, bool boolToChangeTo)
    {
        boolToChange = boolToChangeTo;
    }

    public void SetClimb(bool value)
    {
        isClimbing = value;

        SetAnimationBool("isClimbing", isClimbing);
    }

    public void SetAnimationBool(string name, bool value)
    {
        animator.SetBool(name, value);
    }
}
