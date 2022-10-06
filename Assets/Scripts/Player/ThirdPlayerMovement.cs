using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum HookshotStates
{
    Normal,
    HookshotThrown,
    HookshotFlyingPlayer,
    HookshotPullingObject
}

enum MovementSpeeds
{
    Walking,
    Sprinting
}

public enum GroundStates
{
    Grounded,
    Airborne,
    Gliding
}

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
    private float gravity = -14f;
    private float speed = 8f;
    private float speedBoost = 14f;
    public float jumpHeight = 3f;
    public float fallTimer;
    private float fallTimerMax = 5f;
    private float turnSmoothTime = 0.1f;
    private float glidingSpeed = 1f;
    private float glideTimer;
    private float glideTimerMax = 5f;

    float turnSmoothVelocity;
    private float groundDistance = 0.2f;

    private float moveSpeed = 6f;
    private float sprintSpeed = 12f;

    [SerializeField]
    private float coyoteTimer;

    [SerializeField]
    private float coyoteTime = 0.4f;
    [SerializeField]
    private float coyoteTimerOffset = 0.1f;


    Vector3 velocity;
    private Vector3 hookshotPosition;
    private float hookshotSize;
    [SerializeField]
    private HookshotStates hookshotState;
    [SerializeField]
    private MovementSpeeds moveSpeeds;
    public GroundStates groundState;


    private GameObject grappleHit;
    private readonly float hookshotSpeedMin = 20f;
    private readonly float hookshotSpeedMax = 40f;
    private void Awake()
    {
        hookshotState = HookshotStates.Normal;
        hookshotTransform.gameObject.SetActive(false);
    }

    private void Start()
    {
        fallTimer = fallTimerMax;
        basicMovementScript = GetComponent<ThirdPlayerMovement>();
    }

    void Update()
    {
        if (OM.outfit == Outfits.Utility && groundState == GroundStates.Airborne)
        {
            DisableGrappleInput();

        }
        if (UI.buildPanelStatus || UI.radialMenuStatus || UI.menu == Menus.Paused)
            return;

        switch (hookshotState)
        {
            default:
            case HookshotStates.Normal:
                HandleMovement();
                StartGrapple();

                break;
            case HookshotStates.HookshotThrown:
                HandleHookShotThrow();
                HandleMovement();
                break;
            case HookshotStates.HookshotFlyingPlayer:
                EndCoyoteTimer();
                HandleHookshotPlayerMovement();
                break;
            case HookshotStates.HookshotPullingObject:

                break;

        }
    }
    private void LateUpdate()
    {
        grappleHook.transform.rotation = Camera.main.transform.rotation;
    }
    private void HandleMovement()
    {
        groundState = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask) ? GroundStates.Grounded : GroundStates.Airborne;

        switch (groundState)
        {
            case GroundStates.Grounded:
                ResetCoyoteTimer();
                if (fallTimer <= 0)
                {
                    GM.RespawnPlayer();
                    fallTimer = fallTimerMax;
                    glideTimer = glideTimerMax;
                }
                fallTimer = fallTimerMax;
                glideTimer = glideTimerMax;

                if (velocity.y < 0)
                {
                    velocity.y = -2f;
                }
                break;

            case GroundStates.Airborne:
                {
                    fallTimer -= Time.deltaTime;
                    CoyoteTimer();
                }
                break;
            case GroundStates.Gliding:
                Debug.Log("is gliding");

                break;
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



        if (Input.GetButtonDown("Jump") && coyoteTimer >= 0)
        {
            StartCoroutine(Jump());
        }
        velocity.y += gravity * Time.deltaTime;
        velocity += characterVelocityMomentum;
        controller.Move(velocity * Time.deltaTime);

        gravity = -9.81f;
        if (OM.outfit == Outfits.Utility)
        {
            if (glideTimer > 0 && IM.glide_Input && velocity.y <= 0)
            {
                groundState = GroundStates.Gliding;
                if (glideTimer <= 0)
                {
                    groundState = GroundStates.Airborne;
                    return;
                }
                gravity = 0;
                velocity = new Vector3(velocity.z, -glidingSpeed);
                //velocity.y = Mathf.Sqrt(gravity * -0.1f / jumpHeight);
                glideTimer -= Time.deltaTime;
                fallTimer = fallTimerMax;
            }
        }
        if (groundState == GroundStates.Airborne) return;
        HandleSprinting();
    }

    private void ResetCoyoteTimer()
    {
        coyoteTimer = coyoteTime;
    }
    private void EndCoyoteTimer()
    {
        coyoteTimer = 0f;
    }
    private void CoyoteTimer()
    {
        coyoteTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && groundState == GroundStates.Airborne && coyoteTimer > 0)
        {
            Jump();
        }
    }

    private IEnumerator Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        yield return new WaitForSeconds(coyoteTimerOffset);
        coyoteTimer = 0f;
    }


    private void HandleSprinting()
    {
        if (hookshotState == HookshotStates.HookshotThrown || groundState == GroundStates.Gliding)
        {
            moveSpeeds = MovementSpeeds.Walking;

        }

        if (groundState == GroundStates.Grounded || groundState == GroundStates.Airborne)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                moveSpeeds = MovementSpeeds.Sprinting;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                moveSpeeds = MovementSpeeds.Walking;
            }
        }

        switch (moveSpeeds)
        {
            case MovementSpeeds.Walking:
                basicMovementScript.speed = moveSpeed;
                break;
            case MovementSpeeds.Sprinting:
                basicMovementScript.speed = sprintSpeed;
                break;
        }
    }
    #region GrappleHook
    private void StartGrapple()
    {

        if (!UI.buildPanelStatus)
        {
            if (OM.outfit == Outfits.Utility)
            {

                if (groundState == GroundStates.Airborne)
                {
                    return;
                }
                if (IM.rClick_Input)
                {
                    if (Physics.Raycast(grapplePoint.transform.position, grapplePoint.transform.forward, out RaycastHit raycastHit, 100))
                    {
                        grappleHit = raycastHit.collider.gameObject;
                        Debug.Log(grappleHit.name);
                        if (grappleHit.CompareTag("Non-Grappleable-Surface"))
                        {
                            StopHookshot();
                            IM.rClick_Input = false;
                            return;
                        }

                        debugHitPointTransform.position = raycastHit.point;
                        hookshotPosition = raycastHit.point;
                        hookshotSize = 0f;
                        hookshotTransform.gameObject.SetActive(true);
                        hookshotTransform.localScale = Vector3.zero;
                        hookshotState = HookshotStates.HookshotThrown;
                    }
                    IM.rClick_Input = false;
                }
            }
        }
    }

    private void HandleHookShotThrow()
    {
        hookshotTransform.LookAt(hookshotPosition);
        float hookshotThrowSpeed = 150f;
        hookshotSize += hookshotThrowSpeed * Time.deltaTime;
        hookshotTransform.localScale = new Vector3(1, 1, hookshotSize);



        if (grappleHit.CompareTag("Lilypad"))
        {
            hookshotState = HookshotStates.HookshotPullingObject;
            LilyPadPull(grappleHit);
        }

        if (hookshotSize >= Vector3.Distance(transform.position, hookshotPosition))
        {
            hookshotState = HookshotStates.HookshotFlyingPlayer;
        }
    }

    private void HandleHookshotPlayerMovement()
    {
        //hookshot looks at position of target hit
        hookshotTransform.LookAt(hookshotPosition);
        Vector3 hookshotDir = (hookshotPosition - transform.position).normalized;

        //the speed the hookshot pulls the player
        float hookshotSpeed = Mathf.Clamp(Vector3.Distance(transform.position, hookshotPosition), hookshotSpeedMin, hookshotSpeedMax);
        float hookshotSpeedMultiplier = 2f;

        //move the player in direction of hookshot at hookshot speed
        controller.Move(hookshotDir * hookshotSpeed * hookshotSpeedMultiplier * Time.deltaTime);

        hookshotSize = Vector3.Distance(transform.position, hookshotPosition);
        hookshotTransform.localScale = new Vector3(1, 1, hookshotSize);
        float reachedHookshotPositionDistance = 2f;
        if (Vector3.Distance(transform.position, hookshotPosition) < reachedHookshotPositionDistance)
        {
            //Reached hookshot position
            glideTimer = glideTimerMax;
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

    private void LilyPadPull(GameObject lilyPad)
    {
        Debug.Log("Pulling lilypad");
        lilyPad.transform.position = Vector3.MoveTowards(lilyPad.transform.position, gameObject.transform.position, 1 * Time.deltaTime);
        //Move lily pad towards player
        /*
        * move lily towards player on the x,z
        */
    }

    public void StopHookshot()
    {
        hookshotState = HookshotStates.Normal;
        hookshotTransform.gameObject.SetActive(false);
    }
    #endregion
    private void DisableGrappleInput()
    {
        IM.rClick_Input = false;
    }

}