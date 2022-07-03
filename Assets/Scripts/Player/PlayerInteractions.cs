using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerInteractions : GameBehaviour
{
    public bool canPickUp;
    public GameObject objectToPickUp;
    public int lightPickUpValue = 8;

    private void Update()
    {
        if (IM.interact_Input)
        {
            if (objectToPickUp == null)
            {
                IM.interact_Input = false;
                return;
            }

            if (canPickUp)
            {
                if (objectToPickUp.CompareTag("LightPickUp"))
                {
                    FL.ChangeIntensity(lightPickUpValue);
                }

                if (objectToPickUp.CompareTag("SmallRock"))
                {
                    GM.smallRocksCollected +=1;
                    UI.UpdateSmallRocksCollectedText();
                    Debug.Log(GM.smallRocksCollected);

                }

                //Debug.Log("Picked up small object");
                Destroy(objectToPickUp);
                canPickUp = false;
                objectToPickUp = null;
                IM.interact_Input = false;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SmallRock"))
        {
            //Debug.Log("collide with small rock");
            other.GetComponent<Outline>().enabled = true;
            canPickUp = true;
            objectToPickUp = other.gameObject;
        }
        if (other.CompareTag("LightPickUp"))
        {
            Debug.Log("Light PickUp detected");
            other.GetComponent<Outline>().enabled = true;
            canPickUp = true;
            objectToPickUp = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SmallRock") || other.CompareTag("LightPickUp"))
        {
            other.GetComponent<Outline>().enabled = false;
            canPickUp = false;
            objectToPickUp = null;
        }
    }
}
