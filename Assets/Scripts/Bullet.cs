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
    private float _bulletTimerMax, _bulletTimer;
    private Rigidbody _rb;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _bulletTimerMax = 3f;
        _bulletTimer = _bulletTimerMax;
        
    }

    private void Update()
    {
        Physics.IgnoreLayerCollision(10, 7);
        _bulletTimer = Mathf.Clamp(_bulletTimer, 0, _bulletTimerMax);
        _bulletTimer -= Time.deltaTime;
        if(_bulletTimer <= 0)
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

        if(collision.gameObject.CompareTag("Mechanics"))
        {
            return;
        }
        Debug.Log(collision.gameObject);
        //Debug.Log(collision.gameObject.name);
        //gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        StartCoroutine(nameof(DestroyBullet));
        //StartCoroutine(DestroyBullet());
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
