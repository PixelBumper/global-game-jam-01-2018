using System;
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
		var timer = TimeSpan.FromSeconds(_gameSettings.GameDurationSeconds);

		CountDown = Observable.Interval(TimeSpan.FromSeconds(1))
			.TakeUntil(Observable.Timer(timer))
			.Select(counter => timer.Subtract(TimeSpan.FromSeconds(counter)));

		CountDown.Subscribe(
				timeLeft => Debug.Log("Still alive for ... " + timeLeft),
				Debug.LogException,
				() => SceneManager.LoadScene("ScoreScene")
			);

		WorldChanges = Observable.EveryUpdate()
			.Where(_ => Input.GetKeyDown(KeyCode.Space))
			.Scan(EWorldStatus.Living, (status, l) => status);

		ScoreChanges = Observable.Interval(TimeSpan.FromSeconds(1));
	}
}
