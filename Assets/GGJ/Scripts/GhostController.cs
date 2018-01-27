using System.Collections;
using System.Collections.Generic;
using GGJ.Scripts.ScriptableObjects;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(AudioSource))]
public class GhostController : MonoBehaviour
{
    [SerializeField]
    private float _mumblingRange;

    [SerializeField]
    private float _rangeForPlayingSequence;

    [SerializeField]
    private SingleNoteConfiguration[] _noteConfiguration;

    [SerializeField]
    [TooltipAttribute("the time that has to pass between two sequence ")]
    private float _delayBetweenSequenceRepetition;

    [SerializeField]
    private AudioClip _mumblingSound;

    private float _deltaSinceLastFinishedSequence;

    private GameObject _currentPlayingNote;
    private SpriteRenderer _spriteRendererOfCurrentNote;

    private AudioSource _audioSource;
    private SphereCollider _sphereCollider;

    private int _nextNoteToPlay = 0;
    private bool _isMumbling = false;
    private bool _isPlayingSequence = false;

    // Use this for initialization
    void Start()
    {
        if (_noteConfiguration == null || _noteConfiguration.Length == 0)
        {
            // TODO init ghost with random note
        }

        _deltaSinceLastFinishedSequence = _delayBetweenSequenceRepetition;
        _audioSource = GetComponent<AudioSource>();

        GetComponent<SphereCollider>().radius = _mumblingRange;

        _currentPlayingNote = new GameObject(
            "note symbols", 
            typeof(SpriteRenderer), 
            typeof(RendererLayerOrderingHandler));

        _currentPlayingNote.SetActive(false);
        var transform = _currentPlayingNote.transform;
        transform.SetParent(this.transform);

        transform.eulerAngles = new Vector3(90, 0, 0);
        transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1);
        
        _spriteRendererOfCurrentNote = _currentPlayingNote.GetComponent<SpriteRenderer>();
        _spriteRendererOfCurrentNote.sprite = _noteConfiguration[_nextNoteToPlay].Sprite;
    }

    private void FixedUpdate()
    {
        
        if (_isMumbling && _audioSource.isPlaying == false)
        {
            _currentPlayingNote.SetActive(false);
            _audioSource.PlayOneShot(_mumblingSound);
        }

        if (_isPlayingSequence && _audioSource.isPlaying == false)
        {
            _deltaSinceLastFinishedSequence += Time.deltaTime;
            if (_deltaSinceLastFinishedSequence < _delayBetweenSequenceRepetition)
            {
                // hide note in case we are waiting for the delay to be over
                _currentPlayingNote.SetActive(false);
            }
            else
            {
                // play the next note in the sequence
                _currentPlayingNote.SetActive(true);
                _spriteRendererOfCurrentNote.sprite = _noteConfiguration[_nextNoteToPlay].Sprite;
                _audioSource.PlayOneShot(_noteConfiguration[_nextNoteToPlay].Note);
                _nextNoteToPlay++;

                // if this was the last note start waiting
                if (_nextNoteToPlay >= _noteConfiguration.Length)
                {
                    _nextNoteToPlay = 0;
                    _deltaSinceLastFinishedSequence = 0;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if ("Player".Equals(other.gameObject.tag))
        {
            _isMumbling = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ("Player".Equals(other.tag))
        {
            if (Vector3.Distance(other.transform.position, transform.position) < _rangeForPlayingSequence)
            {
                if (_isMumbling)
                {
                    _nextNoteToPlay = 0;
                    _audioSource.Stop();
                    _currentPlayingNote.SetActive(true);
                    _isMumbling = false;
                    _isPlayingSequence = true;
                }

            }
            else if (_isPlayingSequence)
            {
                _audioSource.Stop();
                _isMumbling = true;
                _currentPlayingNote.SetActive(false);
                _isPlayingSequence = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ("Player".Equals(other.gameObject.tag))
        {
            _isMumbling = false;
        }
    }
}