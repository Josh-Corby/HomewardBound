using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPlayerMovement : GameBehaviour<ThirdPlayerMovement>
{
    [Header("References")]
    public CharacterController controller;
    public Transform cam;
    public Vector3 characterVelocityMomentum;
    [SerializeField]
    private Transform debugHitPointTransform;
    [SerializeField]
    private Transform hookshotTransform;
    public GameObject grapplePoint;
    public GameObject grappleHook;
    public Transform groundCheck;
    public LayerMask groundMask;

    ThirdPlayerMovement basicMovementScript;

    //Character modifiers
    private float gravity = -9.81f;
    private float speed = 8f;
    private float speedBoost = 12f;
    public float jumpHeight = 3f;
    public float fallTimer;
    private float fallTimerMax = 2f;
    private float turnSmoothTime = 0.1f;
    private float glidingSpeed = 1f;
    private float glideTimer;
    private float glideTimerMax = 3f;

    float turnSmoothVelocity;
    private float groundDistance = 0.4f;
    
    //Bools
    [HideInInspector]
    public bool isGrounded;
    private bool isGliding;

    
    Vector3 velocity;
    private Vector3 hookshotPosition;
    private float hookshotSize;

    private State state;
    private enum State
    {
        Normal,
        HookshotThrown,
        HookshotFlyingPlayer
    }

    private void Awake()
    {
        state = State.Normal;
        hookshotTransform.gameObject.SetActive(false);
    }

    private void Start()
    {
        fallTimer = fallTimerMax;
        basicMovementScript = GetComponent<ThirdPlayerMovement>();
    }

    void Update()
    {
        if (OM.outfits != Outfits.Grapple || !isGrounded)
        {
            DisableGrappleInput();

        }
        


        switch (state)
        {
            default:
            case State.Normal:
                
                HandleMovement();
                if(OM.outfits == Outfits.Grapple)
                {
                    if (BM.haveGrappleHook)
                    {
                        StartGrapple();
                    }
                }
                
                    break;
            case State.HookshotThrown:
                HandleHookShotThrow();
                HandleMovement();
                break;
            case State.HookshotFlyingPlayer:
                HandleHookshotMovement();
                break;
        }

        
    }
    private void LateUpdate()
    {
        grappleHook.transform.rotation = Camera.main.transform.rotation;
  
    }
    private void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded)
        {
            if (fallTimer <= 0)
            {
                GM.RespawnPlayer();
                fallTimer = fallTimerMax;
            }
            fallTimer = fallTimerMax;
            glideTimer = glideTimerMax;
        }
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if(!isGrounded && !isGliding)
        {
            fallTimer -= Time.deltaTime;
            
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        velocity += characterVelocityMomentum;

        controller.Move(velocity * Time.deltaTime);

        //if (characterVelocityMomentum.magnitude >= 0f)
        //{
        //    float momentumDrag = 3f;
        //    characterVelocityMomentum -= momentumDrag * Time.deltaTime * characterVelocityMomentum;
            
        //}
        //if (characterVelocityMomentum.magnitude < 0f)
        //{
        //    characterVelocityMomentum = Vector3.zero;
        //}

        if(OM.outfits == Outfits.Glider)
        {
            if (glideTimer > 0 && BM.haveGlider && IM.glide_Input && velocity.y <= 0)
            {
                isGliding = true;
                if (glideTimer <= 0)
                {
                    isGliding = false;
                    return;
                }
                gravity = 0;
                velocity = new Vector3(velocity.z, -glidingSpeed);
                //velocity.y = Mathf.Sqrt(gravity * -0.1f / jumpHeight);
                glideTimer -= Time.deltaTime;
            }
        
        }
        else
        {
            gravity = -9.81f;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("Sprinitng");
            basicMovementScript.speed += speedBoost;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            basicMovementScript.speed -= speedBoost;


    }
    private void StartGrapple()
    {
        if(!isGrounded)
        {
            return;
        }
        if (IM.rClick_Input)
        {
            if (Physics.Raycast(grapplePoint.transform.position, grapplePoint.transform.forward, out RaycastHit raycastHit, 100))
            {
                debugHitPointTransform.position = raycastHit.point;
                
                hookshotPosition = raycastHit.point;
                hookshotSize = 0f;
                hookshotTransform.gameObject.SetActive(true);
                hookshotTransform.localScale = Vector3.zero;
                state = State.HookshotThrown;
            }
            IM.rClick_Input = false;
        }
       
    }

    private void HandleHookShotThrow()
    {
        hookshotTransform.LookAt(hookshotPosition);

        float hookshotThrowSpeed = 150f;
        hookshotSize += hookshotThrowSpeed * Time.deltaTime;
        hookshotTransform.localScale = new Vector3(1, 1, hookshotSize);

        if(hookshotSize >= Vector3.Distance(transform.position, hookshotPosition))
        {
            state = State.HookshotFlyingPlayer;
        }
    }

    private void HandleHookshotMovement()
    {
        hookshotTransform.LookAt(hookshotPosition);

        Vector3 hookshotDir = (hookshotPosition - transform.position).normalized;


        float hookshotSpeedMin = 20f;
        float hookshotSpeedMax = 40f;
        float hookshotSpeed = Mathf.Clamp(Vector3.Distance(transform.position, hookshotPosition), hookshotSpeedMin, hookshotSpeedMax);
        float hookshotSpeedMultiplier = 2f;
        //Move character controller
        controller.Move(hookshotDir * hookshotSpeed * hookshotSpeedMultiplier * Time.deltaTime);

        hookshotSize = Vector3.Distance(transform.position, hookshotPosition);
        hookshotTransform.localScale = new Vector3(1, 1, hookshotSize);
        float reachedHookshotPositionDistance = 2f;
        if(Vector3.Distance(transform.position, hookshotPosition) < reachedHookshotPositionDistance)
        {
            //Reached hookshot position
            StopHookshot();
        }

        if (IM.rClick_Input)
        {        
           IM.rClick_Input = false;
            StopHookshot();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //float momentumExtraSpeed = 7f;
            //float jumpSpeed = 40f;
            //characterVelocityMomentum = hookshotDir / 10;
            //characterVelocityMomentum += Vector3.up * jumpSpeed;
            StopHookshot();           
        }
    }

    private void StopHookshot()
    {
        state = State.Normal;
        hookshotTransform.gameObject.SetActive(false);
    }

    private void DisableGrappleInput()
    {
        IM.rClick_Input = false;
    }
}