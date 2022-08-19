using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleObjectOutline : GameBehaviour
{
    private Outline objectOutline;
    private float outlineWidth = 8;

    private float lerpTIme;

    private void Awake()
    {
        objectOutline = GetComponent<Outline>();
        objectOutline.enabled = false;
    }

    private void Update()
    {
        if (OM.outfit == Outfits.Utility)
        {
            objectOutline.enabled = true;
            objectOutline.OutlineWidth = Mathf.PingPong(Time.time * 10, outlineWidth);
        }
        else
            objectOutline.enabled = false;
    }
}
