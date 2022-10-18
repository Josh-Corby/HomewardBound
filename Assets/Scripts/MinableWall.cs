using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinableWall : GameBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public IEnumerator Break()
    {
        animator.Play("Break");
        
        yield return new WaitForSeconds(1.3f);

        IZ.ClearObject();
    }
}
