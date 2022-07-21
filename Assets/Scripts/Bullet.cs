using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float bulletTimerMax, bulletTimer;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        bulletTimerMax = 3f;
        bulletTimer = bulletTimerMax;
        Physics.IgnoreLayerCollision(10, 7);
    }

    private void Update()
    {
        bulletTimer = Mathf.Clamp(bulletTimer, 0, bulletTimerMax);
        bulletTimer -= Time.deltaTime;
        if(bulletTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
