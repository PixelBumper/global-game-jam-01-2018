using System;
using UniRx;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int _gameDurationSeconds = 2 * 60;

	private void Start ()
	{
		var timer = TimeSpan.FromSeconds(_gameDurationSeconds);

		Observable.Interval(TimeSpan.FromSeconds(1))
			.TakeUntil(Observable.Timer(timer))
			.Select(counter => timer.Subtract(TimeSpan.FromSeconds(counter)))
			.Subscribe(
				timeLeft => Debug.Log("Still alive for " + timeLeft),
				Debug.LogException,
				() => Debug.Log("We're dead :(")
			);
	}
}
