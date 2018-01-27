using GGJ.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private SingleNoteConfiguration[] _allNotes;
    [SerializeField] private AudioClip[] _allMumbles;
    [SerializeField] private HumanController _humanControllerPrefab;
    [SerializeField] private GhostController _ghostControllerPrefab;

    public GameSettings GameSettings { get { return _gameSettings; } }

    public Subject<Unit> _ScoreChangesSubject = new Subject<Unit>();

    public IObservable<HelpScore> ScoreChanges
    {
        get
        {
            return _ScoreChangesSubject
                .StartWith(Unit.Default)
                .Scan(0, (i, unit) => i + i)
                .Select(current => new HelpScore(current, GameSettings.MaximumHelps));
        }
    }

    public IObservable<TimeSpan> CountDown;
    public IObservable<EWorldStatus> WorldChanges;
    private HashSet<AudioClip> _allMumblesHashed;
    private Dictionary<AudioClip, Tuple<HumanController, GhostController>> _mumblingOwners;

    private void Awake()
    {
        _mumblingOwners = new Dictionary<AudioClip, Tuple<HumanController, GhostController>>();

        var initialWorld = EWorldStatus.Living;

        WorldChanges = Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .Scan(initialWorld, (status, l) => status.Advance())
            .StartWith(initialWorld);

        var timer = TimeSpan.FromSeconds(_gameSettings.GameDurationSeconds);

        CountDown = Observable.Interval(TimeSpan.FromSeconds(1))
            .Select(seconds => seconds + 1)
            .StartWith(0) // There's no way to specify the initialDelay so we'll fake it.
            .TakeUntil(Observable.Timer(timer))
            .Select(counter => timer.Subtract(TimeSpan.FromSeconds(counter + 1)));

        ScoreChanges.CombineLatest(CountDown, (score, time) => new KeyValuePair<HelpScore, TimeSpan>(score, time))
            .Where(pair => pair.Key.isFinished() || pair.Value.TotalSeconds == 0)
            .Subscribe(pair => SceneManager.LoadScene("ScoreScene"));

        _allMumblesHashed = new HashSet<AudioClip>(_allMumbles);
        foreach (var mumble in _allMumblesHashed)
        {
            var notesConfiguration = GetRandomNotesConfiguration();
            var humanInstance = Instantiate(_humanControllerPrefab);
            humanInstance.OnSuccess += OnHumanSuccess;
            humanInstance.SetNoteConfiguration(notesConfiguration);
            humanInstance.SetMumbling(mumble);
            var ghostInstance = Instantiate(_ghostControllerPrefab);
            ghostInstance.SetMumbling(mumble);
            ghostInstance.SetNoteConfiguration(notesConfiguration);

            _mumblingOwners[mumble] = new Tuple<HumanController, GhostController>(humanInstance, ghostInstance);
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
        Destroy(characters.Item1.gameObject);
        Destroy(characters.Item2.gameObject);
        Rescued();
    }

    public void Rescued()
    {
        _ScoreChangesSubject.OnNext(Unit.Default);
    }
}
