using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float bulletTimerMax, bulletTimer;

    private void Start()
    {
        bulletTimerMax = 3f;
        bulletTimer = bulletTimerMax;
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
