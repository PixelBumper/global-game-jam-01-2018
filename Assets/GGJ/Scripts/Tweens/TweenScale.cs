using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class TweenScale : MonoBehaviour
{
    public enum LoopType
    {
        NoLoop,
        Repeat,
        Yoyo,
    }

    public enum LoopState
    {
        Forward,
        Backward,
    }

    public bool AutoStart = true;
    public LoopType Loop = LoopType.NoLoop;
    public float Duration = 1;
    public Vector3 EndValue = Vector3.one;
    private Vector3 _initialValue;
    private Vector3 _targetValue;
    private LoopState _state = LoopState.Forward;

    private void Start ()
    {
        _initialValue = transform.localScale;
        _targetValue = EndValue;
        if (AutoStart)
        {
            StartTween();
        }
	}
    
    public void StartTween()
    {
        var tweener = transform.DOScale(_targetValue, Duration);
        tweener.Play();
        tweener.onComplete = TweenCompleted;
    }

    private void TweenCompleted()
    {
        switch(Loop)
        {
            case LoopType.Repeat:
                StartTween();
                break;
            case LoopType.Yoyo:
                _state = _state == LoopState.Forward ? LoopState.Backward : LoopState.Forward;
                _targetValue = _state == LoopState.Forward ? EndValue : _initialValue;
                StartTween();
                break;
        }
    }
}
