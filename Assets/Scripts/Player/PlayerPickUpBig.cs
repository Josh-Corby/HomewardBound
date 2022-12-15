using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpBig : MonoBehaviour
{
    public Transform theDes;

    void OnMouseDown()
    {
        GetComponent<Rigidbody>().useGravity = false;
        this.transform.position = theDes.position;
        GetComponent<Rigidbody>().freezeRotation = true;

        this.transform.parent = GameObject.Find("Destination").transform;
    }

    void OnMouseUp()
    {
        this.transform.parent = null;
        GetComponent<Rigidbody>().freezeRotation = false;
        GetComponent<Rigidbody>().useGravity = true;
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        GetComponent<Rigidbody>().useGravity = false;
    //        this.transform.position = theDes.position;
    //        this.transform.parent = GameObject.Find("Destination").transform;
    //    }

    //    if (Input.GetKeyUp(KeyCode.E))
    //    {
    //        this.transform.parent = null;
    //        GetComponent<Rigidbody>().useGravity = true;
    //    }


    //}
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("BigObject") && (Input.GetKeyDown(KeyCode.E)))
    //    {
    //        Debug.Log("collide with big object");
    //        other.GetComponent<Outline>().enabled = true;

    //        GetComponent<Rigidbody>().useGravity = false;
    //        this.transform.position = theDes.position;
    //        this.transform.parent = GameObject.Find("Destination").transform;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("BigObject") && (Input.GetKeyUp(KeyCode.E)))
    //    {
    //        other.GetComponent<Outline>().enabled = false;

    //        this.transform.parent = null;
    //        GetComponent<Rigidbody>().useGravity = true;
    //    }
    //}
}
