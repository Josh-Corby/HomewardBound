using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDetection : MonoBehaviour
{
    Color startColor = Color.green;
    Color endColor = Color.red;
    float lerpTime = 1f;
    bool lerp = false;

    public MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer.material.color = Color.green;
    }

    private void Update()
    {
        if (lerp)
        {
            meshRenderer.material.color = Color.Lerp(meshRenderer.material.color, endColor, Time.deltaTime * lerpTime);
        }
        if (!lerp)
        {
            meshRenderer.material.color = Color.Lerp(meshRenderer.material.color, startColor, Time.deltaTime * lerpTime);
        }

        if(meshRenderer.material.color == Color.red)
        {
            Debug.Log("Player Caught!");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Detected");
            lerp = true;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player Not Detected");
            lerp = false;

        }
    }
}
