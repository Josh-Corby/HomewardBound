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

    private void Start()
    {
        GameManager.OnPlayerRespawn += EnableObject;
    }
    private void EnableObject()
    {
        gameObject.SetActive(true);
        animator.Play("New State");
    }
    public IEnumerator Break()
    {
        animator.Play("Break");
        
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);  
    }
}
