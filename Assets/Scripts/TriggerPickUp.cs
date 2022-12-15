using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPickUp : MonoBehaviour
{
    public Transform theDes;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKey(KeyCode.E))
        {
            GetComponent<Rigidbody>().useGravity = false;
            this.transform.position = theDes.position;
            GetComponent<Rigidbody>().freezeRotation = true;

            this.transform.parent = GameObject.Find("Destination").transform;
        }

        else
        {
            this.transform.parent = null;
            GetComponent<Rigidbody>().freezeRotation = false;
            GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
