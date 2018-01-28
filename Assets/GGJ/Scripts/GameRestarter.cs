using System;
using GGJ.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestarter : MonoBehaviour {
	private DateTime _started;

	private void Awake()
	{
		GameObject.Find("timeLeft").GetComponent<TextMeshProUGUI>().text = InputController.Instance._timeLeft.ToString().Substring(0,8);
		GameObject.Find("totalHelps").GetComponent<TextMeshProUGUI>().text = InputController.Instance._score;
	}

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
