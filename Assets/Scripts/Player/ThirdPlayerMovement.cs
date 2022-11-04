using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

enum HookshotStates
{
    Default,
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
    public Camera cam;
    [SerializeField]
    private Transform cameraLook;
    public Vector3 characterVelocityMomentum;
    [SerializeField]
    private Transform debugHitPointTransform;
    [SerializeField]
    private Transform hookshotTransform;
    public GameObject grapplePoint;
    public GameObject grappleHook;
    public Transform groundCheck;
    public LayerMask groundMask;
    public LayerMask buildMask;

    ThirdPlayerMovement basicMovementScript;

    //Character modifiers
    [SerializeField]
    private float gravity;
    [SerializeField]
    private float defaultGravity = -19.6f;
    private float speed = 8f;
    public float jumpHeight = 3f;
    public float fallTimer;
    public float fallTimerMax = 5f;
    //private float glidingSpeed = 1f;
    private float glideTimer;
    private float glideTimerMax = 5f;
    private float groundDistance = 0.8f;
    private float moveSpeed = 6f;
    private float sprintSpeed = 12f;
    private float coyoteTimer;
    [SerializeField]
    private float coyoteTime = 0.4f;
    [SerializeField]
    private float coyoteTimerOffset = 0.1f;
    [SerializeField]
    Vector3 moveDir;
    Vector3 velocity;

    private Vector3 hookshotPosition;
    private float hookshotSize;
    [SerializeField]
    private HookshotStates hookshotState;
    [SerializeField]
    private MovementSpeeds moveSpeeds;
    public GroundStates groundState;

    bool buildState;

    [SerializeField]
    private GameObject grappleHitObject;
    private readonly float hookshotSpeedMin = 20f;
    private readonly float hookshotSpeedMax = 40f;
    private readonly float pullSpeedMin = 1;
    private readonly float pullSpeedMax = 2;
    private float grappleRange = 50f;
    [SerializeField]
    LayerMask grappleMask;

    private Vector3 groundBox =  new Vector3(0.3f, 0.5f, 0.3f);

    public GameObject LilypadOffset;

    private void Awake()
    {
        hookshotState = HookshotStates.Default;
        hookshotTransform.gameObject.SetActive(false);
    }

    private void Start()
    {
        fallTimer = fallTimerMax;
        basicMovementScript = GetComponent<ThirdPlayerMovement>();
    }

    void Update()
    {


        LookFoward();
        if (OM.outfit == Outfits.Utility && groundState == GroundStates.Airborne)
        {
            DisableGrappleInput();

        }
        if (/*UI.buildPanelStatus || UI.radialMenuStatus || */UI.menu == Menus.Paused || UI.menu == Menus.Paused)
            return;
    }
    private void LateUpdate()
    {
        if (UI.menu == Menus.Radial) return;

        switch (hookshotState)
        {
            case HookshotStates.Default:
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
                PullLilypadTowardsPlayer();
                HandleMovement();
                break;
        }

        grappleHook.transform.rotation = Camera.main.transform.rotation;

        moveDir = Vector3.zero;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;


            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        }


        if (LilypadOffset != null)
        {
            Vector3 translation = LilypadOffset.transform.position - transform.position;

            controller.Move(translation + (moveDir.normalized * speed * Time.deltaTime));

            LilypadOffset.transform.position = transform.position;
        }

        if (LilypadOffset == null)
        {
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

   
    private void LookFoward()
    {
        if (!IZ.isRolling)
        {
            var lookPos = cam.transform.position - cameraLook.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = rotation;
        }
     
    }
    private void HandleMovement()
    {
        //groundState = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask) ? GroundStates.Grounded : GroundStates.Airborne;

        groundState = Physics.CheckBox(groundCheck.position, groundBox, Quaternion.identity, groundMask) ? GroundStates.Grounded : GroundStates.Airborne;
        BM.onBuildObject = Physics.CheckSphere(groundCheck.position, groundDistance, buildMask);

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
                    velocity.y = -3f;
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
        if (Input.GetButtonDown("Jump") && coyoteTimer >= 0 && !UI.paused)
        {
            if (IZ.isRolling) return;
            StartCoroutine(Jump());
        }
        velocity.y += gravity * Time.deltaTime;
        velocity += characterVelocityMomentum;
        controller.Move(velocity * Time.deltaTime);

        gravity = defaultGravity;
       
        if (groundState == GroundStates.Airborne) return;

        if(!IZ.isRolling)
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
            Debug.Log("jump");
            StartCoroutine(Jump());
        }
    }
    private IEnumerator Jump()
    {
        AM.Jump();
        LilypadOffset = null;
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        yield return new WaitForSeconds(coyoteTimerOffset);
        coyoteTimer = 0f;

        yield return new WaitForSeconds(0.5f);
        AM.StopJumping();
    }
    private void HandleSprinting()
    {
        if (hookshotState == HookshotStates.HookshotThrown || groundState == GroundStates.Gliding)
        {
            moveSpeeds = MovementSpeeds.Walking;

        }

        if (groundState == GroundStates.Grounded)
        {
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeeds = MovementSpeeds.Walking;
            }
        }

        if (groundState == GroundStates.Grounded || groundState == GroundStates.Airborne)
        {

            if (Input.GetKey(KeyCode.LeftShift))
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
        if (!UI.paused)
        {
            //if (!UI.buildPanelStatus)
            
                if (OM.outfit == Outfits.Utility)
                {

                    if (groundState == GroundStates.Airborne)
                    {
                        return;
                    }
                    if (IM.rClick_Input)
                    {
                        //Find the exact hit position using a raycast
                        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Just a ray through the middle of your current view
                        RaycastHit hit;

                        //check if ray hits something
                        Vector3 targetPoint;
                        if (Physics.Raycast(ray, out hit, Mathf.Infinity, grappleMask))
                            targetPoint = hit.point;
                        else
                            targetPoint = ray.GetPoint(grappleRange); //Just a point far away from the player

                        //Calculate direction from attackPoint to targetPoint
                        Vector3 directionWithoutSpread = targetPoint - grapplePoint.transform.position;

                        if (Physics.Raycast(grapplePoint.transform.position, directionWithoutSpread, out hit, grappleRange, grappleMask))

                        //if (Physics.Raycast(grapplePoint.transform.position, directionWithoutSpread, out RaycastHit raycastHit, 100))
                        {
                            grappleHitObject = hit.collider.gameObject;
                            //Debug.Log(grappleHitObject.name);
                            if (grappleHitObject.CompareTag("Non-Grappleable-Surface") || grappleHitObject.CompareTag("FallingLilyPad"))
                            {
                                StopHookshot();
                                IM.rClick_Input = false;
                                return;
                            }

                            debugHitPointTransform.position = hit.point;
                            hookshotPosition = hit.point;
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


        if (grappleHitObject.CompareTag("Lilypad"))
        {
            if (hookshotSize >= Vector3.Distance(transform.position, hookshotPosition))
            {
                grapplePoint.transform.parent = grappleHitObject.transform;
                hookshotState = HookshotStates.HookshotPullingObject;
                return;
            }
        }
        if (hookshotSize >= Vector3.Distance(transform.position, hookshotPosition))
        {
            hookshotState = HookshotStates.HookshotFlyingPlayer;
        }
    }

    private void HandleHookshotPlayerMovement()
    {
        LilypadOffset = null;
        //hookshot looks at position of target hit
        hookshotTransform.LookAt(hookshotPosition);
        Vector3 hookshotDir = (hookshotPosition - transform.position).normalized;

        //the speed the hookshot pulls the player
        float hookshotSpeed = Mathf.Clamp(Vector3.Distance(transform.position, hookshotPosition), hookshotSpeedMin, hookshotSpeedMax);
        float hookshotSpeedMultiplier = 2f;

        //move the player in direction of hookshot at hookshot speed
        controller.Move(hookshotDir * hookshotSpeed * hookshotSpeedMultiplier * Time.deltaTime);

        ManageHookshotSize();
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

    private void PullLilypadTowardsPlayer()
    {
        if (UI.paused) return;

        float hookshotSpeed = Mathf.Clamp(Vector3.Distance(transform.position, grappleHitObject.transform.position), pullSpeedMin, pullSpeedMax);
        float hookshotSpeedMultiplier = 0.1f;

        Vector3 pullDestination = new Vector3(gameObject.transform.position.x, grappleHitObject.transform.position.y, gameObject.transform.position.z);
        grappleHitObject.transform.position = Vector3.MoveTowards(grappleHitObject.transform.position, pullDestination, hookshotSpeed * hookshotSpeedMultiplier);
        //ManageHookshotSize();

        if (Vector3.Distance(new Vector3(gameObject.transform.position.x, 0f, gameObject.transform.position.z), new Vector3(grappleHitObject.transform.position.x, 0f, grappleHitObject.transform.position.z)) < 2f)
        {
            StopHookshot();
            grappleHitObject.GetComponent<LilyPad>().ResetLilyPad();
        }
    }

    private void ManageHookshotSize()
    {
        hookshotSize = Vector3.Distance(transform.position, hookshotPosition);
        hookshotTransform.localScale = new Vector3(1, 1, hookshotSize);
    }

    public void StopHookshot()
    {
        hookshotState = HookshotStates.Default;
        hookshotTransform.gameObject.SetActive(false);
        grapplePoint.transform.parent = gameObject.transform;
        grapplePoint.transform.position = transform.position;
    }
    #endregion
    private void DisableGrappleInput()
    {
        IM.rClick_Input = false;
    }
}