using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebTrigger : GameBehaviour
{
    private readonly int _movementDivide = 2;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == TPM.gameObject || other.gameObject.GetComponent<Bullet>())
        {
            TPM.DivideVelocity(_movementDivide);
            Destroy(gameObject);

        }
    }
}
