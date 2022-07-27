using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Outfits { 
    Miner,
    Builder,
    Slingshot,
    Utility
}


public class OutfitManager : GameBehaviour<OutfitManager>
{
    public Outfits outfit;
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
      
    }

    public void ChangeOutfits (int outfitValue)
    {
        if (!canChangeOutfits)
        {
            return;
        }
        Outfits CurrentOutfit = (Outfits)outfitValue;


        switch (CurrentOutfit)
        {
            case Outfits.Miner:
                outfit = Outfits.Miner;
                break;

            case Outfits.Builder:
                outfit = Outfits.Builder;
                break;

            case Outfits.Slingshot:
                outfit = Outfits.Slingshot;
                break;

            case Outfits.Utility:
                outfit = Outfits.Utility;
                break;
        }

        UI.UpdateControlText();
    }
}
