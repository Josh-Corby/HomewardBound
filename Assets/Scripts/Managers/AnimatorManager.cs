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
        if(TPM.groundState == GroundStates.Grounded)
        {
            animator.SetBool("isGrounded", true);
        }

        if (TPM.groundState != GroundStates.Grounded)
        {
            animator.SetBool("isGrounded", false);
        }
        if (isJumping == true) return;

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            animator.SetBool("isWalkingBack", isWalkingBack= false);
            animator.SetBool("isWalking", isWalking = false);
            return;
        }

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            animator.SetBool("isSteppingLeft", isSteppingLeft = false);
            animator.SetBool("isSteppingRight", isSteppingRight = false);
            return;
        }

        isSteppingLeft = Input.GetKey(KeyCode.A);
        animator.SetBool("isSteppingLeft", isSteppingLeft);

        isSteppingRight = Input.GetKey(KeyCode.D);
        animator.SetBool("isSteppingRight", isSteppingRight);

        animator.SetBool("isWalking", isWalking = Input.GetKey(KeyCode.W));
        animator.SetBool("isWalkingBack", isWalkingBack = Input.GetKey(KeyCode.S));


        isTurningLeft = IM.cameraInput.x < 0;
        isTurningRight = IM.cameraInput.x > 0;

        animator.SetBool("isTurningLeft", isTurningLeft);
        animator.SetBool("isTurningRight", isTurningRight);
    }

    public void Jump()
    {
        animator.SetBool("isJumping", true);
    }

    public void StopJumping()
    {
        animator.SetBool("isJumping", false);
    }
    public void Grounded()
    {
        animator.SetBool("isJumping", false);
    }

    public void SetBool(string name, bool value)
    {
        animator.SetBool(name, value);
    }
}
