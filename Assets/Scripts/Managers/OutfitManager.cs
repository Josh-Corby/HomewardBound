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



    private void Start()
    {
        canChangeOutfits = true;

        PlayerManager.OnToolSelected += ChangeOutfits;
    }

    /// <summary>
    /// Change outfits to outfit that matches the value passed in
    /// </summary>
    /// <param name="outfitValue"> Index value of outfit enum case to be changed to</param>
    public void ChangeOutfits (int outfitValue)
    {
  
        if (!canChangeOutfits)
        {
            return;
        }

        if(outfitValue >=1 && outfitValue <= 3)
        {
            outfitValue = 1;
        }

        if(outfitValue >= 4)
        {
            outfitValue -= 2;
        }
        Debug.Log(outfitValue);

        Outfits CurrentOutfit = (Outfits)outfitValue;

        Debug.Log(CurrentOutfit);
        if (CurrentOutfit!= Outfits.Builder)
        {
            BM.CancelBuilding();
        }

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
                Debug.Log("Grapple selected");
                break;
            case Outfits.None:
                outfit = Outfits.None;
                break;
        }

        //Update controls UI to match current outfit controls
        //UI.UpdateControlText();
    }
}
