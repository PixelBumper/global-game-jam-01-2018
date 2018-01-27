using System;
using TMPro;
using UniRx;
using UnityEngine;

public class GameTimerToText : MonoBehaviour
{
    private TextMeshProUGUI _textComp;

    private void Start()
    {
        _textComp = GetComponent<TextMeshProUGUI>();
        var gameManager = FindObjectOfType<GameManager>();
        gameManager.CountDown.Subscribe(OnCountdown);
    }

    private void OnCountdown(TimeSpan timeLeft)
    {
        _textComp.text = string.Format("{0}", timeLeft);
    }
}
