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
		var interval = TimeSpan.FromSeconds(1);

		Observable.Interval(interval)
			.TakeUntil(Observable.Timer(timer))
			.Select(_ => timer.Subtract(interval))
			.Subscribe(
				timeLeft => Debug.Log("Still alive for " + timeLeft),
				Debug.LogException,
				_ => Debug.Log("We're dead :(")
			);
	}
}
