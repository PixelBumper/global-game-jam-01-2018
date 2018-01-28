using System;
using System.Collections;
using System.Collections.Generic;
using GGJ.Scripts.ScriptableObjects;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(AudioSource))]
public class GhostController : MonoBehaviour
{
    private List<SingleNoteConfiguration> _noteConfiguration;

    [SerializeField]
    [TooltipAttribute("the time that has to pass between two sequence ")]
    private float _delayBetweenSequenceRepetition;

    [SerializeField]
    private AudioClip _mumblingSound;

    private float _deltaSinceLastFinishedSequence;

    private GameObject _speakBubble;
    private GameObject _currentPlayingNote;
    private SpriteRenderer _spriteRendererOfCurrentNote;

    private AudioSource _audioSource;
    private SphereCollider _sphereCollider;

    private int _nextNoteToPlay = 0;
    private bool _isMumbling = true;
    private bool _isPlayingSequence = false;
    private AudioClip _myGhostMumblingClip;

    // Use this for initialization
    private void Start()
    {
        var ghostSoundProvider = GhostSoundProvider.FindOnScene();
        var myGhostIndex = ghostSoundProvider.GetAvailableIndex();

        _myGhostMumblingClip = ghostSoundProvider.AudioClips[myGhostIndex];
        Instantiate(ghostSoundProvider.Animators[myGhostIndex], transform);

        _deltaSinceLastFinishedSequence = _delayBetweenSequenceRepetition;
        _audioSource = GetComponent<AudioSource>();

        _speakBubble = transform.Find("SpeakBubble").gameObject;
        _currentPlayingNote = _speakBubble.transform.Find("CurrentNote").gameObject;

        _speakBubble.SetActive(false);

        _spriteRendererOfCurrentNote = _currentPlayingNote.GetComponent<SpriteRenderer>();
        _spriteRendererOfCurrentNote.sprite = _noteConfiguration[_nextNoteToPlay].Sprite;
    }

    public void SetNoteConfiguration(List<SingleNoteConfiguration> newConfig)
    {
        _noteConfiguration = newConfig;
    }

    private void FixedUpdate()
    {
        if (_isMumbling && _audioSource.isPlaying == false)
        {
            _speakBubble.SetActive(false);
            _audioSource.PlayOneShot(_myGhostMumblingClip);
        }

        if (_isPlayingSequence && _audioSource.isPlaying == false)
        {
            _deltaSinceLastFinishedSequence += Time.deltaTime;
            if (_deltaSinceLastFinishedSequence < _delayBetweenSequenceRepetition)
            {
                // hide note in case we are waiting for the delay to be over
                _speakBubble.SetActive(false);
            }
            else
            {
                // play the next note in the sequence
                _speakBubble.SetActive(true);
                _spriteRendererOfCurrentNote.sprite = _noteConfiguration[_nextNoteToPlay].Sprite;
                _audioSource.PlayOneShot(_noteConfiguration[_nextNoteToPlay].Note);
                _nextNoteToPlay++;

                // if this was the last note start waiting
                if (_nextNoteToPlay >= _noteConfiguration.Count)
                {
                    _nextNoteToPlay = 0;
                    _deltaSinceLastFinishedSequence = 0;
                }
            }
        }
    }

    public void SetMumbling(AudioClip mumble)
    {
        _mumblingSound = mumble;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ("Player".Equals(other.gameObject.tag))
        {
            StartMessageSequence();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ("Player".Equals(other.gameObject.tag))
        {
            StartMumbling();
        }
    }

    private void StartMumbling()
    {
        _audioSource.Stop();
        _isMumbling = true;
        _speakBubble.SetActive(false);
        _isPlayingSequence = false;
    }

    private void StartMessageSequence()
    {
        _nextNoteToPlay = 0;
        _audioSource.Stop();
        _speakBubble.SetActive(true);
        _isMumbling = false;
        _isPlayingSequence = true;
    }
}