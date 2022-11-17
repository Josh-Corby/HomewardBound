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

    private void Start()
    {
        transform.DOScale(_tweenScale, _tweenTime).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        //transform.DORotate(new Vector3(transform.rotation.x, 360, 0), _tweenTime * 3f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);

    }

}
