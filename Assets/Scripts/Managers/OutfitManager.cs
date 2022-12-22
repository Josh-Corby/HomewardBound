using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Outfits 
{ 
    Builder,
    Slingshot,
    None
}


public class OutfitManager : GameBehaviour<OutfitManager>
{
    public Outfits outfit;
    public bool canChangeOutfits;
    //public bool haveSlingshot = true;

    [SerializeField]
    private GameObject SlingshotObject;
    private void Start()
    {
        canChangeOutfits = true;

        PlayerManager.OnToolSelected += ChangeOutfits;
        SlingshotObject.SetActive(false);
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

        if(outfitValue >=1 && outfitValue <= 2)
        {
            outfitValue = 0;
        }

        if(outfitValue == 3)
        {
            outfitValue = 1;
        }
        if(outfitValue >= 4)
        {
            outfitValue =2;
        }
        //Debug.Log(outfitValue);

        Outfits CurrentOutfit = (Outfits)outfitValue;


        switch (CurrentOutfit)
        {
            case Outfits.Builder:
                outfit = Outfits.Builder;
                SlingshotObject.SetActive(false);
                break;

            case Outfits.Slingshot:
                outfit = Outfits.Slingshot;
                SlingshotObject.SetActive(true);
                break;

            case Outfits.None:
                outfit = Outfits.None;
                break;
        }

    }
}
