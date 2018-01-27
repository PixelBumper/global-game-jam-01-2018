﻿using System;
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
	public IObservable<EFire> FireChanges;

	private void Awake()
	{
		var timer = TimeSpan.FromSeconds(_gameSettings.GameDurationSeconds);

		CountDown = Observable.Interval(TimeSpan.FromSeconds(1))
			.TakeUntil(Observable.Timer(timer))
			.Select(counter => timer.Subtract(TimeSpan.FromSeconds(counter)))
			.StartWith(timer); // Immediately sent it off so everyone has a default value.

		CountDown.Subscribe(
			timeLeft => { /** No-op. */ },
			Debug.LogException,
			() => SceneManager.LoadScene("ScoreScene")
		);

		WorldChanges = Observable.EveryUpdate()
			.Where(_ => Input.GetKeyDown(KeyCode.Space))
			.Scan(EWorldStatus.Living, (status, l) => status.Advance());

		ScoreChanges = Observable.Interval(TimeSpan.FromSeconds(1));

		FireChanges = Observable.EveryUpdate()
			.SelectMany(
				Enum.GetValues(typeof(EFire))
					.Cast<EFire>()
					.ToObservable()
					.Where(fire => Math.Abs(Input.GetAxis(fire.ToString())) > 0.2f && Input.GetButton(fire.ToString()))
			);
	}
}
