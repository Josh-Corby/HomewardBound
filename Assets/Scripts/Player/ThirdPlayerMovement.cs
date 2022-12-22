using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

enum MovementSpeeds
{
    Walking,
    Sprinting
}
public enum GroundStates
{
    Grounded,
    Airborne,
}
public class ThirdPlayerMovement : GameBehaviour<ThirdPlayerMovement>
{

    public static event Action<bool> OnSprintingStateChange = null;
    public static event Action<bool> OnGroundedStateChange = null;
    public static event Action<bool> OnJump = null;

    [Header("References")]
    public CharacterController Controller;
    public Camera Cam;
    [SerializeField]
    private Transform _cameraLookRight;
    public Vector3 CharacterVelocityMomentum;

    public GameObject GrappleHook;
    public Transform GroundCheck;
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

    [SerializeField]
    private MovementSpeeds moveSpeeds;
    public GroundStates groundState;

    private Vector3 groundBox = new Vector3(0.3f, 0.5f, 0.3f);
    private bool isSprinting;


    private void Start()
    {
        fallTimer = fallTimerMax;
        basicMovementScript = GetComponent<ThirdPlayerMovement>();
    }

    void Update()
    {
        LookFoward();

        if (UI.menu == Menus.Paused)
            return;
    }
    private void LateUpdate()
    {
        HandleMovement();
        GrappleHook.transform.rotation = Camera.main.transform.rotation;
        moveDir = Vector3.zero;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Cam.transform.eulerAngles.y;
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }
        Controller.Move(moveDir.normalized * speed * Time.deltaTime);

    }


    private void LookFoward()
    {
        if (Vector3.Distance(Cam.transform.position, _cameraLookRight.transform.position) <= 0.8f)
        {
            return;
        }

        var lookPos = Cam.transform.position - _cameraLookRight.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;


    }
    private void HandleMovement()
    {

        groundState = Physics.CheckBox(GroundCheck.position, groundBox, Quaternion.identity, groundMask) ? GroundStates.Grounded : GroundStates.Airborne;
        BM.OnBuildObject = Physics.CheckSphere(GroundCheck.position, groundDistance, buildMask);

        switch (groundState)
        {
            case GroundStates.Grounded:
                OnGroundedStateChange?.Invoke(true);
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
                    OnGroundedStateChange?.Invoke(false);
                    CheckGroundRays();
                    fallTimer -= Time.deltaTime;
                    CoyoteTimer();
                }
                break;

        }

        if (Input.GetButtonDown("Jump") && coyoteTimer >= 0 && !UI.paused)
        {
            StartCoroutine(Jump());
        }
        velocity.y += gravity * Time.deltaTime;
        velocity += CharacterVelocityMomentum;
        Controller.Move(velocity * Time.deltaTime);
        gravity = defaultGravity;

        if (groundState == GroundStates.Grounded)
        {
            HandleSprinting();
        }
    }

    public void DivideVelocity(int value)
    {
        velocity.y /= value;
    }


    private Ray CreateEdgeCheckRay(Vector3 dir)
    {
        Vector3 rayPos = transform.position;
        rayPos.y -= Controller.height / 4f;
        Debug.DrawLine(rayPos, dir, Color.red);
        return new Ray(rayPos, dir);
    }

    private Ray[] CreateSideRays()
    {
        Vector3 rayStart = transform.position;
        rayStart.y -= (Controller.height / 4f);
        Ray[] sideRays = new Ray[4];
        sideRays[0] = CreateEdgeCheckRay(rayStart + Vector3.right);
        sideRays[1] = CreateEdgeCheckRay(rayStart + Vector3.left);
        sideRays[2] = CreateEdgeCheckRay(rayStart + Vector3.forward);
        sideRays[3] = CreateEdgeCheckRay(rayStart + Vector3.back);
        return sideRays;
    }

    private void CheckGroundRays()
    {
        Vector3 inEdgeMovement = Vector3.zero;
        foreach (Ray ray in CreateSideRays())
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 1, groundMask))
            {
                inEdgeMovement += (-ray.direction);
            }
            Controller.Move(inEdgeMovement * Time.deltaTime);
        }
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
            StartCoroutine(Jump());
        }
    }
    private IEnumerator Jump()
    {
        OnJump?.Invoke(true);
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        yield return new WaitForSeconds(coyoteTimerOffset);
        coyoteTimer = 0f;

        yield return new WaitForSeconds(0.5f);
        OnJump?.Invoke(false);
    }
    private void HandleSprinting()
    {
        if (groundState == GroundStates.Grounded || groundState == GroundStates.Airborne)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                StopSprinting();
            }

            //if(!Input.GetKey(KeyCode.W) && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
            //{
            //    StopSprinting();
            //}

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                ToggleSprint();
            }
        }
    }

    public void StopSprinting()
    {
        isSprinting = false;
        basicMovementScript.speed = isSprinting ? sprintSpeed : moveSpeed;
        OnSprintingStateChange?.Invoke(isSprinting);
    }
    private void ToggleSprint()
    {
        isSprinting = !isSprinting;
        basicMovementScript.speed = isSprinting ? sprintSpeed : moveSpeed;
        OnSprintingStateChange?.Invoke(isSprinting);
    }
}
