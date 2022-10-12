using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderClimb : GameBehaviour<LadderClimb>
{
    //public float open = 100f;
    //public float range = 0.5f;
    //public bool TouchingWall = false;
    //public float UpwardSpeed = 3.3f;
    //public Camera LadderCam;

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    Shoot();

    //    if (Input.GetKey("w") & TouchingWall == true)
    //    {
    //        StartCoroutine(StartClimb());
    //    }

    //    if (Input.GetKeyUp("w"))
    //    {
    //        GetComponent<Rigidbody>().isKinematic = false;
    //        TouchingWall = false;
    //    }
    //}

    //void Shoot()
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(LadderCam.transform.position, LadderCam.transform.forward, out hit, range))
    //    {
    //        Target target = hit.transform.GetComponent<Target>();
    //        if (target != null)
    //        {
    //            TouchingWall = true;
    //        }
    //    }
    //}

    //IEnumerator StartClimb()
    //{
    //    transform.position += Vector3.up * Time.deltaTime * UpwardSpeed;
    //    GetComponent<Rigidbody>().isKinematic = true;
    //    yield return new WaitForSeconds(0.85f);
    //    TouchingWall = false;
    //    GetComponent<Rigidbody>().isKinematic = false;
    //}


    public Transform PlayerController;
    public bool inside = false;
    public float speedUpDown = 3.2f;
    public ThirdPlayerMovement TPMMovement;

    void Start()
    {
        inside = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Ladder")
        {
            TPM.groundState = GroundStates.Grounded;
            TPM.enabled = false;
            inside = !inside;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Ladder")
        { 
            TPM.enabled = true;
            TPM.groundState = GroundStates.Grounded;
            inside = !inside;
        }
    }

    void Update()
    {
        if (inside == true && Input.GetKey("w"))
        {
            PlayerController.transform.position += Vector3.up / speedUpDown;
            Debug.Log("Climbing Up");
        }

        if (inside == true && Input.GetKey("s"))
        {
            PlayerController.transform.position += Vector3.down / speedUpDown;
            Debug.Log("Climbing Down");
        }
    }

}