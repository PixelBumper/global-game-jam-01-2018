using GGJ.Scripts.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "GGJ/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    [Header("Session")]
    [SerializeField]
    private int _gameDurationSeconds;
    public int GameDurationSeconds { get { return _gameDurationSeconds; }  }
    [Header("Player")]
    [SerializeField]
    private float _playerSpeed = 10;
    public float PlayerSpeed { get { return _playerSpeed; } }

    [Header("Notes")]
    [SerializeField]
    private SingleNoteConfiguration[] _availableNotes;
    public SingleNoteConfiguration[] AvailableNotes
    {
        get { return _availableNotes; }
    }
}
