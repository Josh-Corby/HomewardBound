using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPlayerMovement : GameBehaviour<ThirdPlayerMovement>
{
    public CharacterController controller;
    public Transform cam;
    ThirdPlayerMovement basicMovementScript;

    public float gravity = -9.81f;
    public float speed = 6f;
    public float speedBoost = 12f;
    public float jumpHeight = 3f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool isGrounded;

    public float glidingSpeed = 2f;
    Vector3 velocity;
    public Vector3 characterVelocityMomentum;

    private float glideTimer;
    private float glideTimerMax = 3f;

    private LineRenderer lr;
    public GameObject grapplePoint;
    public LayerMask whatIsGrappleable;

    [SerializeField] 
    private Transform debugHitPointTransform;
    [SerializeField]
    private Transform hookshotTransform;

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
        lr = GetComponent<LineRenderer>();
        state = State.Normal;
        hookshotTransform.gameObject.SetActive(false);
    }

    private void Start()
    {
        basicMovementScript = GetComponent<ThirdPlayerMovement>();
    }

    

    void Update()
    {
        switch (state)
        {
            default:
            case State.Normal:
                
                HandleMovement();
                StartGrapple();
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
        transform.rotation = Camera.main.transform.rotation;
    }
    private void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded)
        {
            glideTimer = glideTimerMax;
        }
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
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

        if (BM.haveGlider && IM.glide_Input && velocity.y <= 0)
        {
            if (glideTimer <= 0)
            {
                return;
            }
            gravity = 0;
            velocity = new Vector3(velocity.z, -glidingSpeed);
            //velocity.y = Mathf.Sqrt(gravity * -0.1f / jumpHeight);
            glideTimer -= Time.deltaTime;
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
        if (IM.rClick_Input)
        {
            if (Physics.Raycast(grapplePoint.transform.position, grapplePoint.transform.forward, out RaycastHit raycastHit))
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

        float hookshotThrowSpeed = 70f;
        hookshotSize += hookshotThrowSpeed * Time.deltaTime;
        hookshotTransform.localScale = new Vector3(1, 1, hookshotSize * 2);

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
}