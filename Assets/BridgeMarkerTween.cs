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
    private Color _canBuildColour;

    [SerializeField]
    private Color _cantBuildColour;

    [SerializeField]
    private float _emissionIntensity;
    private MeshRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
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
            _renderer.material.color = _canBuildColour;
            _renderer.material.SetColor("_EmissionColor", _canBuildColour * _emissionIntensity);
        }

        if (value == false)
        {
            _renderer.material.color = _cantBuildColour;
            _renderer.material.SetColor("_EmissionColor", _cantBuildColour * _emissionIntensity);
        }
    }
}
