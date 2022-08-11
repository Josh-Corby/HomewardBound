using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Outfits { 

    Miner,
    Builder,
    Slingshot,
    Utility,
    None
}


public class OutfitManager : GameBehaviour<OutfitManager>
{
    public Outfits outfit;
    public bool canChangeOutfits;
    public bool haveSlingshot = true;

    /// <summary>
    /// if 0 miner
    /// if 1 builder
    /// if 2 slingshot
    /// if 3 Utility
    /// if 4 None
    /// </summary>

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
            case Outfits.None:
                outfit = Outfits.None;
                break;
        }

        UI.UpdateControlText();
    }
}
