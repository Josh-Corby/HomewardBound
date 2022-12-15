using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilyPad : GameBehaviour
{
    [SerializeField]
    private Vector3 startPosition;
    private float waitTimeMax = 3;
    [SerializeField]
    private float waitTime;

    [SerializeField]
    private float moveSpeed = 0.05f;
    [SerializeField]
    private GameObject Player;
    private Rigidbody rb;
    public bool moveBack;
    public Vector3 moveDirection;
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        Player = TPM.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        waitTime = waitTimeMax;
    }
    private void Update()
    {
        waitTime = Mathf.Clamp(waitTime, 0f, waitTimeMax);
    }
    private void LateUpdate()
    {
        if (UI.paused) return;
        if (moveBack == true)
        {
            MoveBackToStartPosition();
        }
    }
    private void MoveBackToStartPosition()
    {
        moveBack = true;    
        if (waitTime <= 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, moveSpeed);
            if (Vector3.Distance(transform.position, startPosition) <= 0.1f)
            {
                transform.position = startPosition;
                waitTime = waitTimeMax;
                moveBack = false;
            }
        }
        if (waitTime <= 0) return;
        waitTime -= Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            //Debug.Log(other.gameObject.name);

            ResetLilyPad();
            return;
        }
    }

    public void ResetLilyPad()
    {

        waitTime = waitTimeMax;
        MoveBackToStartPosition();    
    }
}
