using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class GameTimerToText : MonoBehaviour
{
    private Text _textComp;

    private void Start()
    {
        _textComp = GetComponent<Text>();
        var gameManager = FindObjectOfType<GameManager>();
        gameManager.CountDown.Subscribe(OnCountdown);
    }


    private void OnCountdown(TimeSpan timeLeft)
    {
        _textComp.text = string.Format("{0}", timeLeft);
    }
}
