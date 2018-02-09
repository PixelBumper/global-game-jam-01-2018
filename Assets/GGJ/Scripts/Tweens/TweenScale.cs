using DG.Tweening;
using UnityEngine;

public class TweenScale : MonoTween
{
    [Header("Scale")]
    public Vector3 InitialValue = Vector3.one;
    public Vector3 EndValue = Vector3.one;

    protected override Tweener GetTweener()
    {
        transform.localScale = InitialValue;
        return transform.DOScale(EndValue, Duration);
    }
}
