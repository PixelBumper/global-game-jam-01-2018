using GGJ.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(AudioSource))]
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
    private SphereCollider _sphereCollider;
    private bool _canMumble = true;
    private IDisposable _dispisable;

    // Use this for initialization
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        GetComponent<SphereCollider>().radius = _mumblingRange;
    }

    public void SetNoteConfiguration(List<SingleNoteConfiguration> newConfig)
    {
        _noteConfiguration = newConfig;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ("Player".Equals(other.gameObject.tag))
        {
            if(_canMumble)
            {
                _canMumble = false;

                _dispisable = Observable.Interval(TimeSpan.FromSeconds(_mumblingSound.length))
                    .Subscribe(_ => { }, Debug.LogException, OnMumbleFinished);

                _audioSource.PlayOneShot(_mumblingSound);
            }
        }
    }

    private void OnMumbleFinished()
    {
        Debug.Log("OnMumbleFinished");
        _canMumble = true;
    }

    private void OnDestroy()
    {
        if(_dispisable != null)
        {
            _dispisable.Dispose();
        }
    }

    public void SetMumbling(AudioClip mumble)
    {
        _mumblingSound = mumble;
        GameObject prefab;
        if(_charactersMap.MumbleToHumans.TryGetValue(mumble, out prefab))
        {
            Instantiate(prefab, transform);
        }
        else
        {
            Debug.LogError("No human matching that mumble");
        }
    }
}
