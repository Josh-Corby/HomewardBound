using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class GUITween : MonoBehaviour
{
    private Vector3 _destination;
    [SerializeField]
    private float _tweenTime;

    // Start is called before the first frame update
    void Start()
    {
        _destination = new Vector3(transform.position.x, transform.position.y + 0.75f, transform.position.z);
        transform.DOMove(_destination, _tweenTime).SetEase(Ease.InOutSine).SetLoops(-1,LoopType.Yoyo);

        transform.DORotate(new Vector3(0, 360, 0), _tweenTime * 3f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

}
