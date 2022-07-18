using UnityEngine;

public class PlayerManager : GameBehaviour<PlayerManager>
{
    CameraManager cameraManager;
    //Animator animator;

    public bool isInteracting;
    public bool isClimbing;

    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        //animator = GetComponent<Animator>();
        isClimbing = false;
    }

    //private void Update()
    //{
    //    IM.HandleAllInputs();
    //}

    private void FixedUpdate()
    {
        //PL.HandleAllMovement();
    }

    private void LateUpdate()
    {
        //cameraManager.HandleAllCameraMovement();

        //isInteracting = animator.GetBool("isInteracting");
        //PL.isJumping = animator.GetBool("isJumping");
        //animator.SetBool("isGrounded", PL.isGrounded);
    }
}
