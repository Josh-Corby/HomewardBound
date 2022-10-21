using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BridgeBuild : GameBehaviour
{
    [SerializeField]
    private GameObject currentExtension;
    [SerializeField]
    private int extensionCount;
    [SerializeField]
    private List<GameObject> BridgeSegments = new List<GameObject>();


    private MeshRenderer renderer;
    private Color baseColour;

    private bool currentBuildObject;

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();

        extensionCount = 0;
        currentExtension = null;

        foreach (var extension in BridgeSegments)
        {
            extension.SetActive(false);
        }
    }
    void Update()
    {
        currentBuildObject = gameObject == BM.buildingObject ? true : false;


        if (currentBuildObject == false)
        {
            ChangeColourOfBridge(baseColour);
        }



        if (currentBuildObject == true)
        {
            if (BM.canBuild)
            {
                ChangeColourOfBridge(Color.blue);
            }

            if (!BM.canBuild)
            {
                ChangeColourOfBridge(Color.red);
            }

            extensionCount = Mathf.Clamp(extensionCount, 0, BridgeSegments.Count);
            if (this.gameObject == BM.buildingObject)
            {
                if (Input.mouseScrollDelta.y > 0)
                {
                    if (extensionCount == BridgeSegments.Count)
                    {
                        return;
                    }
                    extensionCount += 1;
                    currentExtension = BridgeSegments[extensionCount - 1];
                    currentExtension.SetActive(true);
                }
                if (Input.mouseScrollDelta.y < 0)
                {
                    if (currentExtension == null) return;

                    currentExtension.SetActive(false);
                    extensionCount -= 1;

                    if (extensionCount <= 0)
                    {
                        currentExtension = null;
                        return;
                    }
                    currentExtension = BridgeSegments[extensionCount - 1];
                }
            }
        }
    }

    private void ChangeColourOfBridge(Color colour)
    {
        renderer.material.color = colour;
        foreach (GameObject extension in BridgeSegments)
        {
            extension.GetComponent<MeshRenderer>().material.color = colour;
        }
    }
}

