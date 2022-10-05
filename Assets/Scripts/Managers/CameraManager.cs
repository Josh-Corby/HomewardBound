using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : GameBehaviour<CameraManager>
{
    public Camera cam;

    float xRotation;
    private void Update()
    {
        
    }

    private void ClampCamera()
    {
        xRotation = Mathf.Clamp(xRotation, -75f, 38f);
    }
}
