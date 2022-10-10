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

    private Rigidbody rb;

    [HideInInspector]
    public GameObject Player;


    public bool moveBack;

    public Vector3 moveDirection;



    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();

    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        waitTime = waitTimeMax;
    }



    private void LateUpdate()
    {
        if (moveBack == true)
        {
            MoveBackToStartPosition();
        }
    }

    private void MoveBackToStartPosition()
    {
        moveBack = true;
        waitTime -= Time.deltaTime;
        if (waitTime <= 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, 0.01f);


            if (Vector3.Distance(transform.position, startPosition) <= 0.1f)
            {
                transform.position = startPosition;
                waitTime = waitTimeMax;
                moveBack = false;
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.CompareTag("Lilypad"))
        //{
        //    return;
        //}
        if (!other.gameObject.CompareTag("Player"))
        {
            Debug.Log(other.gameObject.name);

            ResetLilyPad();
            return;         
        }

    }



    private void ResetLilyPad()
    {
        
        TPM.StopHookshot();
        waitTime = waitTimeMax;
        MoveBackToStartPosition();
        
    }
}
