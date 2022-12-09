using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BridgeMarkerTween : MonoBehaviour
{
    [SerializeField]
    private Vector3 _tweenScale;
    [SerializeField]
    private float _tweenTime;

    [SerializeField]
    private Color canBuildColour;

    [SerializeField]
    private Color cantBuildColour;

    [SerializeField]
    private float emissionIntensity;
    private MeshRenderer renderer;

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        BuildManager.OnCanBuildStatus += BuildStatusColour;
    }

    private void OnDisable()
    {
        BuildManager.OnCanBuildStatus -= BuildStatusColour;
    }
    private void Start()
    {
        transform.DOScale(_tweenScale, _tweenTime).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
    private void BuildStatusColour(bool value)
    {
        if(value == true)
        {
            renderer.material.color = canBuildColour;
            renderer.material.SetColor("_EmissionColor", canBuildColour * emissionIntensity);
        }

        if (value == false)
        {
            renderer.material.color = cantBuildColour;
            renderer.material.SetColor("_EmissionColor", cantBuildColour * emissionIntensity);
        }
    }
}
