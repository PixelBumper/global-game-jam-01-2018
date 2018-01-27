using System;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int _gameDurationSeconds = 2 * 60;

	public IObservable<long> ScoreChanges;
	public IObservable<TimeSpan> CountDown;
	public IObservable<EWorldStatus> WorldChanges;

	private void Awake()
	{
		var timer = TimeSpan.FromSeconds(_gameDurationSeconds);

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
