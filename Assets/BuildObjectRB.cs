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


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        frozen = false;
        StartCoroutine(FreezeCheck());
    }



    private IEnumerator FreezeCheck()
    {
        yield return new WaitForSeconds(0.1f);

        if (!frozen)
        {
            if (rb.velocity == Vector3.zero)
            {
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
}
