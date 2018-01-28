using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestarter : MonoBehaviour {
	private DateTime _started;

	void Start ()
	{
		_started = DateTime.Now;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && DateTime.Now.Subtract(_started).TotalSeconds > 3)
		{
			SceneManager.LoadScene("Main");
		}
	}
}
