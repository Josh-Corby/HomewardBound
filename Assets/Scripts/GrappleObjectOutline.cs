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

  
}
