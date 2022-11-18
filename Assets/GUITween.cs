using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class GUITween : MonoBehaviour
{
    private Vector3 _destination;
    [SerializeField]
    private float _tweenTime;
    [SerializeField]
    private float _scaleTweenTime;

    void Start()
    {
        _destination = new Vector3(transform.position.x, transform.position.y + 0.75f, transform.position.z);
        transform.DOMove(_destination, _tweenTime).SetEase(Ease.InOutSine).SetLoops(-1,LoopType.Yoyo);

        transform.DORotate(new Vector3(0, 360, 0), _tweenTime * 3f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    public void ScaleDown()
    {
        transform.DOLocalMoveY(0.7f, _scaleTweenTime);
        transform.DOScale(0, _scaleTweenTime);
    }

    public void ScaleUp()
    {
        transform.DOScale(1, _scaleTweenTime);
    }

}
