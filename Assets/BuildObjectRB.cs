using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ObjectType { 
    Bridge,
    Ladder

}

public class BuildObjectRB : MonoBehaviour
{
    [SerializeField]
    private ObjectType objectType;
    public bool frozen;
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private BoxCollider collider;


    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    private void Start()
    {
        frozen = false;
        StartCoroutine(FreezeCheck());
    }



    private IEnumerator FreezeCheck()
    {
        yield return new WaitForSeconds(0.3f);

        if (!frozen)
        {
            if (rb.velocity == Vector3.zero)
            {
                Debug.Log("Object frozen");
                FreezeConstraints();
                frozen = true;
                yield return null;
            }
        }

        StartCoroutine(FreezeCheck());
    }

    public void FreezeConstraints()
    {
        
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void UnFreezeConstraints()
    {

        if(objectType == ObjectType.Bridge)
        {
        rb.constraints = RigidbodyConstraints.None;

        }

        if(objectType == ObjectType.Ladder)
        {
            rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == gameObject) return;
        if (collision.gameObject.CompareTag("Ground")) return;

        Debug.Log(collision.gameObject.name);
        FreezeConstraints();
    }
}
