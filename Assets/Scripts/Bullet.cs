using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletTypes
{
    Basic,
    BerryBomb
}
public class Bullet : MonoBehaviour
{
    private float bulletTimerMax, bulletTimer;
    Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        bulletTimerMax = 3f;
        bulletTimer = bulletTimerMax;
        
    }

    private void Update()
    {
        Physics.IgnoreLayerCollision(10, 7);
        bulletTimer = Mathf.Clamp(bulletTimer, 0, bulletTimerMax);
        bulletTimer -= Time.deltaTime;
        if(bulletTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BreakableWall"))
        {
            Destroy(collision.gameObject);
        }
        Debug.Log(collision.gameObject.name);
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Destroy(gameObject);
        //StartCoroutine(DestroyBullet());
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this);
    }
}
