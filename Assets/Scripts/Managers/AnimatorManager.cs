using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : GameBehaviour<AnimatorManager>
{
    private Animator animator;
    int horizontal, vertical;

    private bool isWalking;
    private bool isRunning;
    private bool isJumping;
    private bool isGrounded;
    private bool isWalkingBack;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    private void Update()
    {
        ManageAnimations();
    }
    private void ManageAnimations()
    {
        isGrounded = TPM.groundState == GroundStates.Grounded;
        animator.SetBool("isGrounded", isGrounded);

        isJumping = false;
        animator.SetBool("isJumping", isJumping);


        if (Input.GetKey(KeyCode.Space))
        {
            isJumping = true;
            animator.SetBool("isJumping", isJumping);
            return;
        }

        if (isJumping == true) return;
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) return;

        isWalking = Input.GetKey(KeyCode.W);
        isRunning = Input.GetKey(KeyCode.LeftShift);
        isWalkingBack = Input.GetKey(KeyCode.S);

        animator.SetBool("isWalkingBack", isWalkingBack);
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isRunning", isRunning);

    }
}
