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
    public SingleNoteConfiguration[] AvailableNotes { get { return _availableNotes; } }

    [Header("Ghost")]
    [TooltipAttribute("defines at which point the player will be able to hear the ghost")]
    [SerializeField]
    private float _rangeForHearingGhostPresence = 3;
    public float RangeForHearingGhostPresence { get { return _rangeForHearingGhostPresence; } }

    [TooltipAttribute("defines at which point the player will be able to hear the ghost sound sequence")]
    [SerializeField]
    private float _rangeForPlayingSequenceOfGhost = 1.5f;
    public float RangeForPlayingSequenceOfGhost { get { return _rangeForPlayingSequenceOfGhost; } }

    [Header("Human")]
    [TooltipAttribute("defines at which point the player will be able to hear the human")]
    [SerializeField]
    private float _rangeForHearingHuman = 3;
    public float RangeForHearingHuman { get { return _rangeForHearingHuman; } }

    [TooltipAttribute("defines at which point the player will be able to input the sequence")]
    [SerializeField]
    private float _rangeForPlayerInput = 1.5f;
    public float RangeForPlayerInput { get { return _rangeForPlayerInput; } }
}
