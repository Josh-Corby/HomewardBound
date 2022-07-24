using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Outfits { 
    Miner,
    Builder,
    Slingshot,
    Grapple,
    Glider,
    Sailor
}

public class OutfitManager : GameBehaviour<OutfitManager>
{
    public Outfits outfits;
    public bool canChangeOutfits;
    /*
     * if 1 miner
     * if 2 builder
     * if 3 slingshot
     * if 4 grapple
     * if 5 glider
     * if 6 sailor
     * 
     * if button is pressed 
     *  disable outfit
     *  set new outfit
    */
    // Start is called before the first frame update

    // Update is called once per frame

    private void Start()
    {
        canChangeOutfits = true;
    }
    void Update()
    {
        ChangeOutfits();
       
    }

    private void ChangeOutfits()
    {
        if (!canChangeOutfits)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            outfits = Outfits.Miner;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            outfits = Outfits.Builder;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            outfits = Outfits.Slingshot;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            outfits = Outfits.Grapple;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            outfits = Outfits.Glider;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            outfits = Outfits.Sailor;
        }
    }
}
