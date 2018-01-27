using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestarter : MonoBehaviour {
	private CompositeDisposable _compositeDisposable = new CompositeDisposable();

	void Start ()
	{
		_compositeDisposable.Add(Observable.EveryUpdate()
			.Where(_ => Input.GetKeyDown(KeyCode.Space))
			.DelaySubscription(TimeSpan.FromSeconds(3))
			.Subscribe(_ => SceneManager.LoadScene("Main")));
	}

	void OnDestroy ()
	{
		_compositeDisposable.Clear();
	}
}
