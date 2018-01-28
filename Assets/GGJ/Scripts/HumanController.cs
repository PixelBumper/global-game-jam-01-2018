using GGJ.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GGJ.Scripts;

[RequireComponent(typeof(AudioSource))]
public class HumanController : MonoBehaviour
{
    [SerializeField] private MumbleCharacterMap _charactersMap;

    [SerializeField] private AudioClip _successSoundClip;
    [SerializeField] private AudioClip _failureSoundClip;
    private GameObject _graphics;
    private AudioClip _mumblingSound;
    private AudioClip _longMumblingSound;
    private AudioClip _ehMumble;
    public event Action<AudioClip> OnSuccess;
    private List<SingleNoteConfiguration> _noteConfiguration;
    private GameObject _currentPlayingNote;
    private AudioSource _audioSource;
    private bool _shouldMumble = true;
    private bool _mumbleFlip = false;

    private int _nextCorrectIndex = 0;
    private int _symbolsEntered = 0;
    private bool _isFinished = false;

    // Use this for initialization
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        InputController.Instance.FireChanges.Subscribe(OnPlayerFireAction);
    }

    public void SetNoteConfiguration(List<SingleNoteConfiguration> newConfig)
    {
        _noteConfiguration = newConfig;
    }

    private void Update()
    {
        if (!_isFinished && _shouldMumble && !_audioSource.isPlaying)
        {
            var currentMumble = _mumbleFlip ? _longMumblingSound : _mumblingSound;
            _mumbleFlip = !_mumbleFlip;
            _audioSource.PlayOneShot(currentMumble);
        }
    }

    public void SetMumbling(AudioClip mumble)
    {
        _mumblingSound = mumble;
        GameObject prefab;
        if (_charactersMap.MumbleToHumans.TryGetValue(mumble, out prefab))
        {
            _graphics = Instantiate(prefab, transform);
        }
        else
        {
            Debug.LogError("No human matching that mumble");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ("Player".Equals(other.gameObject.tag))
        {
            WaitForInputSequence();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ("Player".Equals(other.gameObject.tag))
        {
            StartMumbling();
        }
    }

    private void WaitForInputSequence()
    {
        _nextCorrectIndex = 0;
        _symbolsEntered = 0;
        _shouldMumble = false;
        _audioSource.Stop();
        _audioSource.PlayOneShot(_ehMumble);

    }

    private void OnPlayerFireAction(EFire firePressed)
    {
        if (_shouldMumble || _isFinished || this == null)
        {
            return;
        }

        _symbolsEntered++;
        var indexOfSymbol = -1;
        for (int i = _nextCorrectIndex; i < _noteConfiguration.Count; i++)
        {
            var currentDancingSymbol = _noteConfiguration[i];
            if (firePressed.ToString().Equals(currentDancingSymbol.button))
            {
                indexOfSymbol = i;
                break;
            }
        }

        if (indexOfSymbol == _nextCorrectIndex)
        {
            _nextCorrectIndex++;
        }
        else
        {
            _nextCorrectIndex = 0;
        }

        if (_symbolsEntered == _noteConfiguration.Count)
        {
            if ((indexOfSymbol >= _nextCorrectIndex-1) && (_nextCorrectIndex == _noteConfiguration.Count))
            {
                _isFinished = true;
                AnimateGoodbye();
                return;
            }
            else
            {
                // Failure!
                _audioSource.Stop();
                Invoke("PlayFailure", 0.15f);
                _symbolsEntered = 0;
                _nextCorrectIndex = 0;
                return;
            }
        }
    }

    private void OnDestroy()
    {
        _audioSource.Stop();
        InputController.Instance.FireChanges.Unsbscribe(OnPlayerFireAction);
    }

    private void AnimateGoodbye()
    {
        _audioSource.Stop();
        _audioSource.PlayOneShot(_successSoundClip);
        var tweener = _graphics.transform.DOScale(Vector3.zero, 0.5f);
        tweener.OnComplete(() =>
        {
            _audioSource.Stop();
            OnSuccess(_mumblingSound);
        });
        tweener.Play();
    }

    private void PlayFailure()
    {
        _audioSource.Stop();
        _audioSource.PlayOneShot(_failureSoundClip);
    }

    private void StartMumbling()
    {
        _nextCorrectIndex = 0;
        _symbolsEntered = 0;
        _audioSource.Stop();
        _shouldMumble = true;
    }

    internal void SetLongMumbling(AudioClip longMumble)
    {
        _longMumblingSound = longMumble;
    }

    internal void SetEhMumble(AudioClip ehMumble)
    {
        _ehMumble = ehMumble;
    }
}
