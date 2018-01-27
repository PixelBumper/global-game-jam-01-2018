using System;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;
    public GameSettings GameSettings { get { return _gameSettings; } }
    public IObservable<long> ScoreChanges;
    public IObservable<TimeSpan> CountDown;
    public IObservable<EWorldStatus> WorldChanges;

    private void Awake()
    {
        WorldChanges = Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .Scan(EWorldStatus.Living, (status, l) => status.Advance());

        ScoreChanges = Observable.Interval(TimeSpan.FromSeconds(1));

        var timer = TimeSpan.FromSeconds(_gameSettings.GameDurationSeconds);

        CountDown = Observable.Interval(TimeSpan.FromSeconds(1))
            .Select(seconds => seconds + 1)
            .StartWith(0) // There's no way to specify the initialDelay so we'll fake it.
            .TakeUntil(Observable.Timer(timer))
            .Select(counter => timer.Subtract(TimeSpan.FromSeconds(counter)));

        CountDown.Subscribe(
            timeLeft => { /** No-op. */ },
            Debug.LogException,
            () => SceneManager.LoadScene("ScoreScene")
        );
    }
}
