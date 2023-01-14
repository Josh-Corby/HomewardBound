using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBoardCollision : GameBehaviour
{

    [SerializeField]
    private AudioSource audioSource;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == TPM.gameObject || collision.gameObject.CompareTag("Bullet"))
        audioSource.Play();
    }
}
