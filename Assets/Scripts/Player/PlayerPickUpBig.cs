using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpBig : MonoBehaviour
{
    //public Transform theDes;

    //void OnMouseDown()
    //{
    //    GetComponent<BoxCollider>().enabled = false;
    //    GetComponent<Rigidbody>().useGravity = false;
    //    GetComponent<Rigidbody>().freezeRotation = true;

    //    this.transform.position = theDes.position;
    //    this.transform.parent = GameObject.Find("Destination").transform;
    //}

    //void OnMouseUp()
    //{
    //    this.transform.parent = null;
    //    GetComponent<Rigidbody>().freezeRotation = false;
    //    GetComponent<Rigidbody>().useGravity = true;
    //    GetComponent<BoxCollider>().enabled = true;
    //}

    [Header("Pick Up Settings")]
    [SerializeField] Transform holdArea;
    private GameObject heldObject;
    private Rigidbody heldObjectRB;

    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if(heldObject == null)
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    PickUpObject(hit.transform.gameObject);
                }
            }
            else
            {
                DropObject();
            }
        }

        if(heldObject != null)
        {
            MoveObject();
        }
    }

    void MoveObject()
    {
        if(Vector3.Distance(heldObject.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = (holdArea.position - heldObject.transform.position);
            heldObjectRB.AddForce(moveDirection * pickupForce);
        }
    }

    void PickUpObject(GameObject pickObject)
    {
        if (pickObject.GetComponent<Rigidbody>())
        {
            heldObjectRB = pickObject.GetComponent<Rigidbody>();
            heldObjectRB.useGravity = false;
            heldObjectRB.drag = 10;
            heldObjectRB.constraints = RigidbodyConstraints.FreezeRotation;

            heldObjectRB.transform.parent = holdArea;
            heldObject = pickObject;
        }
    }

    void DropObject()
    {
          heldObjectRB.useGravity = true;
          heldObjectRB.drag = 1;
          heldObjectRB.constraints = RigidbodyConstraints.None;

        heldObject.transform.parent = null;
        heldObject = null;
    }
}
