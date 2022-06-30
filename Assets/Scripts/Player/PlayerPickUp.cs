using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    InputManager inputManager;
    public bool canPickUp;
    public GameObject objectToPickUp;
    private void Awake()
    {
        inputManager = GetComponentInParent<InputManager>();
    }

    private void Update()
    {
        if (inputManager.interact_Input)
        {
            if (objectToPickUp == false)
            {
                inputManager.interact_Input = false;
                return;
            }

            if (canPickUp)
            {
                Debug.Log("Picked up small object");
                Destroy(objectToPickUp);
                canPickUp = false;
                objectToPickUp = null;
                inputManager.interact_Input = false;
            }

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SmallObject"))
        {
            Debug.Log("collide with small object");
            other.GetComponent<Outline>().enabled = true;
            canPickUp = true;
            objectToPickUp = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SmallObject"))
        {
            other.GetComponent<Outline>().enabled = false;
            canPickUp = false;
            objectToPickUp = null;
        }
    }
}
