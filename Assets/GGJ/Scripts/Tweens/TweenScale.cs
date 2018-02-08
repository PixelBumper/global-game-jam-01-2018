using DG.Tweening;
using UnityEngine;

public class TweenScale : MonoTween
{
    [Header("Scale")]
    public Vector3 InitialValue = Vector3.one;
    public Vector3 EndValue = Vector3.one;

    protected override void Start()
    {
        transform.localScale = InitialValue;
        base.Start();
    }

    protected override Tweener GetTweener()
    {
        return transform.DOScale(EndValue, Duration);
    }
}
