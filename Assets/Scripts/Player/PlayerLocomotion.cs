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
    public float leapingVelocity;
    public float fallingVelocity;
    public float rayCastHeightOffSet = 0.5f;
    public LayerMask groundLayer;

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;

    [Header("Movement Speeds")]
    public float mudSpeed = 0.5f;
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5f;
    public float sprintingSpeed = 7f;
    public float rotationSpeed = 15f;

    [Header("Jump Speeds")]
    public float jumpHeight = 3;
    public float gravityIntensity = -15;

    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();
        if (PM.isInteracting) return;

        HandleMovement();
        HandleRotation();
    }
    private void HandleMovement()
    {
        if (isJumping) return;
        moveDirection = new Vector3(cameraObject.forward.x, 0f, cameraObject.forward.z) 
            * IM.verticalInput;
        moveDirection += cameraObject.right * IM.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

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
    private void HandleRotation()
    {
        if (isJumping) return;
        Vector3 targetDirection = Vector3.zero;
        targetDirection = cameraObject.forward * IM.verticalInput;
        targetDirection += cameraObject.right * IM.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, 
            rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        Vector3 targetPosition;
        rayCastOrigin.y += rayCastHeightOffSet;
        targetPosition = transform.position;

        if (!isGrounded && !isJumping)
        {
            if (!PM.isInteracting)
            {
                AM.PlayTargetAnimation("Falling", true);
            }

            inAirTimer += Time.deltaTime;
            playerRigidBody.AddForce(transform.forward * leapingVelocity);
            playerRigidBody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }
        if (Physics.SphereCast(rayCastOrigin, 0.2f, Vector3.down, out hit, 0.5f, groundLayer))
        {
            if(!isGrounded && PM.isInteracting)
            {
                AM.PlayTargetAnimation("LandSoft", true);
            }

            Vector3 rayCastHitPoint = hit.point;
            targetPosition.y = rayCastHitPoint.y;
            inAirTimer = 0;
            isGrounded = true;
            PM.isInteracting = false;
        }
        else
        {
            isGrounded = false; 
        }

        if(isGrounded && !isJumping)
        {
            if(PM.isInteracting || IM.moveAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = targetPosition;
            }
        }
    }

    public void HandleJumping()
    {
        if (isGrounded)
        {
            AM.animator.SetBool("isJumping", true);
            AM.PlayTargetAnimation("Jump", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            playerRigidBody.velocity = playerVelocity;
        }
    }


}
