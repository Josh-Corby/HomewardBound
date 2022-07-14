using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : GameBehaviour

{
    public CharacterController controller;
    public Transform cam;
    PlayerController basicMovementScript;

    public float gravity = -9.81f;

    public float speed = 6f;
    public float speedBoost = 12f;

    public float jumpHeight = 3f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    public float glidingSpeed = 2f;

    Vector3 velocity;

    public bool grounded;
    private void Start()

    {
        basicMovementScript = GetComponent<PlayerController>();
    }
    void Update()
        
    {
        grounded = controller.isGrounded;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

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
        controller.Move(velocity * Time.deltaTime);
        if (IM.glide_Input && velocity.y <= 0)
        {
            gravity = 0;
            velocity = new Vector3(velocity.z, -glidingSpeed);
            //velocity.y = Mathf.Sqrt(gravity * -0.1f / jumpHeight);
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
}