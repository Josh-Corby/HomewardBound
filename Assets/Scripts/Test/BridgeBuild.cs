using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BridgeBuild : GameBehaviour
{
    

    [SerializeField]
    private GameObject currentExtension;
    [SerializeField]
    private int extensionCount;
    [SerializeField]
    private List<GameObject> BridgeSegments = new List<GameObject>();


    private MeshRenderer renderer;
    [SerializeField]
    private Color baseColour;

    [SerializeField]
    private bool currentBuildObject;

    [SerializeField]
    private bool canBuild;


    private bool[] collisionCheck;

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        baseColour = renderer.material.color;
        extensionCount = 0;
        currentExtension = null;

        foreach (var extension in BridgeSegments)
        {
            extension.SetActive(false);
        }
    }

    private void Start()
    {
        BuildObjectTrigger.OnBridgeCollision += StartCollisionCheck;
        canBuild = true;

        for (int i = 0; i < BridgeSegments.Count; i++)
        {

        }
    }

    private void StartCollisionCheck()
    {
        StopCoroutine(CheckSegmentCollisions());

        StartCoroutine(CheckSegmentCollisions());
    }

    private IEnumerator CheckSegmentCollisions()
    {

        yield return new WaitForEndOfFrame();

        foreach (GameObject trigger in BridgeSegments)
        {
            
            canBuild = trigger.GetComponentInChildren<BuildObjectTrigger>().canBuild;
            Debug.Log(trigger.GetComponentInChildren<BuildObjectTrigger>().canBuild);
        }
    }

    void Update()
    {
        if (UI.paused) return;

        BM.canBuild = canBuild;

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

