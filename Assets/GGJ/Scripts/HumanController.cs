using GGJ.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HumanController : MonoBehaviour
{
    [SerializeField] private MumbleCharacterMap _charactersMap;

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

    // Use this for initialization
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void SetNoteConfiguration(List<SingleNoteConfiguration> newConfig)
    {
        _noteConfiguration = newConfig;
    }

    private void Update()
    {
        if (_shouldMumble && !_audioSource.isPlaying)
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
            Instantiate(prefab, transform);
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
        _shouldMumble = false;
        _audioSource.Stop();
        _audioSource.PlayOneShot(_ehMumble);
    }

    private void StartMumbling()
    {
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
