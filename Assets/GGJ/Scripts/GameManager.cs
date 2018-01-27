using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameManager : MonoBehaviour {
	// Use this for initialization
	void Start ()
	{
		var timer = TimeSpan.FromMinutes(3);

		Observable.Interval(TimeSpan.FromSeconds(1))
			.TakeUntil(Observable.Timer(timer))
			.Select(counter => timer.Subtract(TimeSpan.FromSeconds(counter)))
			.Subscribe(
				timeLeft => Debug.Log("Still alive for " + timeLeft),
				Debug.LogException,
				_ => Debug.Log("We're dead :(")
			);
	}
}
