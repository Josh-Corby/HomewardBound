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


    public bool moveBack;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        waitTime = waitTimeMax;
        
    }


    private void Update()
    {
        if (moveBack == true)
        {
            MoveBackToStartPosition();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        TPM.StopHookshot();
        waitTime = waitTimeMax;
        MoveBackToStartPosition();
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
}
