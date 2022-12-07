using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebTrigger : GameBehaviour
{
    private int MovementDivide = 2;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == TPM.gameObject || other.gameObject.GetComponent<Bullet>())
        {
        TPM.DivideVelocity(MovementDivide);
        Destroy(gameObject);

        }
    }
}
