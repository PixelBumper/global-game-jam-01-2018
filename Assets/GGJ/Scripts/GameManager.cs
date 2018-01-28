using GGJ.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using GGJ.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private SingleNoteConfiguration[] _allNotes;
    [SerializeField] private AudioClip[] _allMumbles;
    [SerializeField] private AudioClip[] _allLongMumbles;
    [SerializeField] private AudioClip[] _allEhMumbles;
    [SerializeField] private HumanController _humanControllerPrefab;
    [SerializeField] private GhostController _ghostControllerPrefab;
    [SerializeField] private TupleSpawnPoint[] _spawnPoints;

    public GameSettings GameSettings { get { return _gameSettings; } }

    public IObservable<HelpScore> ScoreChanges { get { return _scoreChanges; } }
    private readonly SimpleObservable<HelpScore> _scoreChanges = new SimpleObservable<HelpScore>();
    private HelpScore _helpScore;

    public IObservable<TimeSpan> CountDownChanges { get { return _countDownChanges; } }
    private readonly SimpleObservable<TimeSpan> _countDownChanges = new SimpleObservable<TimeSpan>();
    private TimeSpan _countDown;

    public IObservable<EWorldStatus> WorldChanges { get { return _worldChanges; } }
    private readonly SimpleObservable<EWorldStatus> _worldChanges = new SimpleObservable<EWorldStatus>();
    private EWorldStatus _currentWorldStatus = EWorldStatus.Living;

    private Dictionary<AudioClip, KeyValuePair<HumanController, GhostController>> _mumblingOwners;

    private void Awake()
    {
        _helpScore = new HelpScore(0, _gameSettings.MaximumHelps);
        _scoreChanges.OnNext(_helpScore);

        _countDown = TimeSpan.FromSeconds(_gameSettings.GameDurationSeconds);
        _countDownChanges.OnNext(_countDown);

        _worldChanges.OnNext(_currentWorldStatus);

        _mumblingOwners = new Dictionary<AudioClip, KeyValuePair<HumanController, GhostController>>();

        List<int> randomIndexList = new List<int>();
        for (int i = 0; i < _allMumbles.Length; i++)
        {
            randomIndexList.Add(i);
        }

        for (int i = 0; i < _allMumbles.Length; i++)
        {
            var randomIndex = UnityEngine.Random.Range(0, randomIndexList.Count);
            var index = randomIndexList[randomIndex];
            randomIndexList.RemoveAt(randomIndex);

            if (i >= _spawnPoints.Length)
            {
                Debug.LogError("You need as many spawn points as mumble sounds");
                return;
            }

            var mumble = _allMumbles[index];
            var longMumble = _allLongMumbles[index];
            var ehMumble = _allEhMumbles[index];

            var positions = _spawnPoints[index];
            var notesConfiguration = GetRandomNotesConfiguration();
            var humanInstance = Instantiate(_humanControllerPrefab);
            humanInstance.transform.position = positions.HumanPosition.position;
            humanInstance.OnSuccess += OnHumanSuccess;
            humanInstance.SetNoteConfiguration(notesConfiguration);
            humanInstance.SetMumbling(mumble);
            humanInstance.SetLongMumbling(longMumble);
            humanInstance.SetEhMumble(ehMumble);
            var ghostInstance = Instantiate(_ghostControllerPrefab);
            ghostInstance.transform.position = positions.GhostPosition.position;
            ghostInstance.SetMumbling(mumble);
            ghostInstance.SetNoteConfiguration(notesConfiguration);

            _mumblingOwners[mumble] = new KeyValuePair<HumanController, GhostController>(humanInstance, ghostInstance);
        }
    }

    void Update()
    {
        _countDown = _countDown.Subtract(TimeSpan.FromSeconds(Time.deltaTime));
        _countDownChanges.OnNext(_countDown);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var findGameObjectWithTag = GameObject.FindWithTag("WorldIndicator");
            if (findGameObjectWithTag != null) findGameObjectWithTag.SetActive(false);

            _currentWorldStatus = _currentWorldStatus.Advance();
            _worldChanges.OnNext(_currentWorldStatus);
        }

        if (_countDown.TotalSeconds <= 0 || _helpScore.isFinished())
        {
            InputController.Instance._score = _helpScore.current + "/" + _helpScore.max;
            InputController.Instance._timeLeft = _countDown.ToString();
            SceneManager.LoadScene("ScoreScene");
        }
    }

    public List<SingleNoteConfiguration> GetRandomNotesConfiguration()
    {
        var result = new List<SingleNoteConfiguration>(4);
        for (int i = 0; i < 4; i++)
        {
            var noteIndex = UnityEngine.Random.Range(0, _allNotes.Length);
            result.Add(_allNotes[noteIndex]);
        }
        return result;
    }

    private void OnHumanSuccess(AudioClip mumble)
    {
        var characters = _mumblingOwners[mumble];
        Destroy(characters.Key.gameObject);
        Destroy(characters.Value.gameObject);
        Rescued();
    }

    public void Rescued()
    {
        _helpScore = new HelpScore(_helpScore.current + 1, _helpScore.max);
        _scoreChanges.OnNext(_helpScore);
    }
}
