using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public abstract class MonoTween : MonoBehaviour
{
    public bool AutoStart = true;
    public float Duration = 1;
    public float Delay = 0.0f;
    public bool IsLooping = false;
    public LoopType LoopType = LoopType.Restart;
    public Ease EasingType = Ease.Linear;
    [SerializeField] protected UnityEvent OnPlayEvent;
    [SerializeField] protected UnityEvent OnRewindEvent;
    [SerializeField] protected UnityEvent OnStepCompleteEvent;

    private Tweener _tweener;

    protected virtual void Start()
    {
        _tweener = GetTweener();
        _tweener.SetDelay(Delay);
        _tweener.SetAutoKill(false);
        _tweener.SetLoops(IsLooping ? -1 : 0, LoopType);
        _tweener.SetEase(EasingType);

        _tweener.onPlay = OnPlay;
        _tweener.onRewind = OnRewind;
        _tweener.onStepComplete = OnStepComplete;

        if (AutoStart)
        {
            Play();
        }
    }

    public void Play()
    {
        _tweener.PlayForward();
    }

    public void Pause()
    {
        _tweener.Pause();
    }

    public void Restart()
    {
        _tweener.Restart();
    }

    private void OnStepComplete()
    {
        OnStepCompleteEvent.Invoke();
    }

    private void OnRewind()
    {
        OnRewindEvent.Invoke();
    }

    private void OnPlay()
    {
        OnPlayEvent.Invoke();
    }

    protected abstract Tweener GetTweener();
}
