using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;
    public GameSettings GameSettings { get { return _gameSettings; } }

    public Subject<Unit> _ScoreChangesSubject = new Subject<Unit>();

    public IObservable<HelpScore> ScoreChanges
    {
        get
        {
            return _ScoreChangesSubject
                .StartWith(Unit.Default)
                .Scan(0, (i, unit) => i + i)
                .Select(current => new HelpScore(current, GameSettings.MaximumHelps));
        }
    }

    public IObservable<TimeSpan> CountDown;
    public IObservable<EWorldStatus> WorldChanges;

    private void Awake()
    {
        var initialWorld = EWorldStatus.Living;

        WorldChanges = Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .Scan(initialWorld, (status, l) => status.Advance())
            .StartWith(initialWorld);

        var timer = TimeSpan.FromSeconds(_gameSettings.GameDurationSeconds);

        CountDown = Observable.Interval(TimeSpan.FromSeconds(1))
            .Select(seconds => seconds + 1)
            .StartWith(0) // There's no way to specify the initialDelay so we'll fake it.
            .TakeUntil(Observable.Timer(timer))
            .Select(counter => timer.Subtract(TimeSpan.FromSeconds(counter + 1)));

        ScoreChanges.CombineLatest(CountDown, (score, time) => new KeyValuePair<HelpScore, TimeSpan>(score, time))
            .Where(pair => pair.Key.isFinished() || pair.Value.TotalSeconds == 0)
            .Subscribe(pair => SceneManager.LoadScene("ScoreScene"));
    }

    public void Rescued()
    {
        _ScoreChangesSubject.OnNext(Unit.Default);
    }
}
