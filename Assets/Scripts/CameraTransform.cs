using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransform : GameBehaviour
{
    public GameObject thirdPersonPlayer;
    private GameObject targetTransform;
    Vector3 yOffset = new Vector3 (0, 0.58f, 0);

    public Vector2 turn;
    
    private void Awake()
    {
        targetTransform = GameObject.Find("CameraLook");
    }

    private void Update()
    {
        if (UI.buildPanelStatus || UI.radialMenuStatus || UI.menu == Menus.Paused)
            return;
        transform.position = thirdPersonPlayer.transform.position + yOffset;
        RotateCamera();
    }

    public void RotateCamera()
    {
        turn.x += Input.GetAxis("Mouse X");
        turn.y += Input.GetAxis("Mouse Y");
        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
    }
}
