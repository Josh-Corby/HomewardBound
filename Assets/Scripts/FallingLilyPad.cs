using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LilypadState
{
    respawned,
    falling,
    hasFallen,

}
public class FallingLilyPad : GameBehaviour
{
    public LilypadState lilypadState;


    [SerializeField]
    private float fallTimer;
    private float fallTime = 4f;


    [SerializeField]
    private bool hasRespawned;


    public bool isFalling;

    [SerializeField]
    private bool isRespawning;

    [SerializeField]
    private float respawnTimer;
    private float respawnTime = 4f;

    private GameObject lilyPadObject;

    private void Awake()
    {     
        lilypadState = LilypadState.respawned;
        lilyPadObject = FindChildGameObjectByName("LilyPadObject");
        ResetLilyPad();
    }
    void Update()
    {

        switch (lilypadState)
        {
            case LilypadState.respawned:
                ResetLilyPad();
                break;

            case LilypadState.falling:
                LilyPadFalling();
                break;

            case LilypadState.hasFallen:
                LilyPadFallen();
                break;

        }
    }

    private void ResetLilyPad()
    {
        if (!hasRespawned)
        {
            //Debug.Log("Lilypad reset");
            lilyPadObject.SetActive(true);
            isFalling = false;
            fallTimer = fallTime;
            isRespawning = false;
            hasRespawned = true;
        }       
    }

    private void LilyPadFalling()
    {
        //Debug.Log("lily pad falling");
        fallTimer -= Time.deltaTime;
        if (fallTimer <= 0)
        {
            lilypadState = LilypadState.hasFallen;
        }
    }
    private void LilyPadFallen()
    {
        if(!isRespawning)
        {
            respawnTimer = respawnTime;
            isRespawning = true;
        }

        lilyPadObject.SetActive(false);
        respawnTimer -= Time.deltaTime;

        if (respawnTimer <= 0)
        {
            hasRespawned = false;
            lilypadState = LilypadState.respawned;
        }
    }

    public void StartFalling()
    {
        lilypadState = LilypadState.falling;
    }
}
