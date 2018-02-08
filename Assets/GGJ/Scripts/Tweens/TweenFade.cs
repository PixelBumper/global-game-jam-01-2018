using DG.Tweening;
using UnityEngine;

public class TweenFade : MonoTween
{
    public float InitialValue = 1.0f;
    public float EndValue = 0.0f;
    private Material _material;

    protected override void Start()
    {
        _material = GetComponent<Renderer>().material;
        var color = _material.color;
        color.a = InitialValue;
        _material.color = color;
        base.Start();
    }

    protected override Tweener GetTweener()
    {
        return GetComponent<Renderer>().material.DOFade(EndValue, Duration);
    }
}
