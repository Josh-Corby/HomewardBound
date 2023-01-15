using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFunctions : MonoBehaviour
{
    [SerializeField]
    private GameObject credits;
    public void EnableCredits()
    {
        credits.SetActive(true);

    }
    public void DisableCredits()
    {
        credits.SetActive(false);
    }
}
