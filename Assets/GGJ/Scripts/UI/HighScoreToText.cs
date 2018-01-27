using TMPro;
using UnityEngine;
using UniRx;

public class HighScoreToText : MonoBehaviour
{
	private TextMeshProUGUI _textComp;

	private void Start()
	{
		_textComp = GetComponent<TextMeshProUGUI>();
		var gameManager = FindObjectOfType<GameManager>();
		gameManager.ScoreChanges.Subscribe(OnScoreChange);
	}

	private void OnScoreChange(HelpScore helpScore)
	{
		_textComp.text = helpScore.ToString();
	}
}
