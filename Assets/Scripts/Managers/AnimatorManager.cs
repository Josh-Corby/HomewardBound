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
    public bool isSprinting;
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
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) return;



        isWalking = Input.GetKey(KeyCode.W);
        isWalkingBack = Input.GetKey(KeyCode.S);

        animator.SetBool("isWalkingBack", isWalkingBack);
        animator.SetBool("isWalking", isWalking);
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
