using GGJ.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HumanController : MonoBehaviour
{
    [SerializeField] private float _mumblingRange;
    [SerializeField] private MumbleCharacterMap _charactersMap;

    private GameObject _graphics;
    private AudioClip _mumblingSound;
    public event Action<AudioClip> OnSuccess;
    private List<SingleNoteConfiguration> _noteConfiguration;
    private GameObject _currentPlayingNote;
    private AudioSource _audioSource;
    private bool _canMumble = true;

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
        if (!_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(_mumblingSound);
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
}
