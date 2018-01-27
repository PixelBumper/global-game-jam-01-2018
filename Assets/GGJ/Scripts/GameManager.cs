using System;
using UniRx;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int _gameDurationSeconds = 2 * 60;

	private CompositeDisposable _compositeDisposable = new CompositeDisposable();

	private void Start()
	{
		var timer = TimeSpan.FromSeconds(_gameDurationSeconds);

		_compositeDisposable.Add(Observable.Interval(TimeSpan.FromSeconds(1))
			.TakeUntil(Observable.Timer(timer))
			.Select(counter => timer.Subtract(TimeSpan.FromSeconds(counter)))
			.Subscribe(
				timeLeft => Debug.Log("Still alive for " + timeLeft),
				Debug.LogException,
				() => Debug.Log("We're dead :(")
			));

		_compositeDisposable.Add(Observable.EveryUpdate()
			.Where(_ => Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
			.Subscribe(
				_ => Debug.Log("Moving up"),
				Debug.LogException
			));

		_compositeDisposable.Add(Observable.EveryUpdate()
			.Where(_ => Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
			.Subscribe(
				_ => Debug.Log("Moving right"),
				Debug.LogException
			));

		_compositeDisposable.Add(Observable.EveryUpdate()
			.Where(_ => Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
			.Subscribe(
				_ => Debug.Log("Moving down"),
				Debug.LogException
			));

		_compositeDisposable.Add(Observable.EveryUpdate()
			.Where(_ => Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
			.Subscribe(
				_ => Debug.Log("Moving left"),
				Debug.LogException
			));
	}

	private void OnDestroy()
	{
		_compositeDisposable.Clear();
	}
}
