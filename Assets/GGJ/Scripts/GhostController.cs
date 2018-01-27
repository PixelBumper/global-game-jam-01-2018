using System.Collections;
using System.Collections.Generic;
using GGJ.Scripts.ScriptableObjects;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(AudioSource))]
public class GhostController : MonoBehaviour
{
	[SerializeField]
	private float _range;
	
	[SerializeField]
	private SingleNoteConfiguration[] _noteConfiguration;

	[SerializeField]
	[TooltipAttribute("the time that has to pass between two sequence ")]
	private float _delayBetweenSequenceRepetition;

	private float _deltaSinceLastFinishedSequence;

	private GameObject _currentPlayingNote;
	private SpriteRenderer _spriteRendererOfCurrentNote;

	private AudioSource _audioSource;
	private SphereCollider _sphereCollider;
	
	private int _nextNoteToPlay = 0;
	private bool _isPlaying;

	// Use this for initialization
	void Start ()
	{
		if (_noteConfiguration == null || _noteConfiguration.Length == 0)
		{
			// TODO init ghost with random note
		}
		_audioSource = GetComponent<AudioSource>();

		GetComponent<SphereCollider>().radius = _range;
		
		_currentPlayingNote = new GameObject();
		
		_currentPlayingNote.SetActive(false);
		var transform = _currentPlayingNote.transform;
		transform.SetParent(this.transform);
		
		transform.position = new Vector2(this.transform.position.x, this.transform.position.y+1);
		_spriteRendererOfCurrentNote = _currentPlayingNote.AddComponent<SpriteRenderer>();
		_spriteRendererOfCurrentNote.sprite = _noteConfiguration[_nextNoteToPlay].Sprite;
	}

	private void FixedUpdate()
	{
		
		if (_isPlaying && _audioSource.isPlaying == false)
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
			_currentPlayingNote.SetActive(true);
			_isPlaying = true;
			_nextNoteToPlay = 0;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if ("Player".Equals(other.gameObject.tag))
		{
			_currentPlayingNote.SetActive(false);
			_isPlaying = false;
			_deltaSinceLastFinishedSequence = 0;
		}
	}
}
