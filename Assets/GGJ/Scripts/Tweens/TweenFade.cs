using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TweenFade : MonoTween
{
    public float InitialValue = 1.0f;
    public float EndValue = 0.0f;
    private Material _material;
    private CanvasGroup _canvasGroup;
    private Graphic _graphics;

    protected override void Start()
    {
        var renderer = GetComponent<Renderer>();
        if(renderer)
        {
            _material = GetComponent<Renderer>().material;
        }

        _canvasGroup = GetComponent<CanvasGroup>();
        _graphics = GetComponent<Graphic>();

        base.Start();
    }

    protected override Tweener GetTweener()
    {
        Color color;
        if(_canvasGroup != null)
        {
            _canvasGroup.alpha = InitialValue;
            return _canvasGroup.DOFade(EndValue, Duration);
        }

        if(_graphics != null)
        {
            color = _graphics.color;
            color.a = InitialValue;
            _graphics.color = color;
            return _graphics.DOFade(EndValue, Duration);
        }

        color = _material.color;
        color.a = InitialValue;
        _material.color = color;
        return _material.DOFade(EndValue, Duration);
    }
}
