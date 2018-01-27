using System.Collections;
using System.Collections.Generic;
using GGJ.Scripts.ScriptableObjects;
using UnityEngine;

public class GhostController : MonoBehaviour
{
	[SerializeField]
	private SingleNoteConfiguration[] _noteConfiguration;

	private GameObject _currentPlayingNote;
	private SpriteRenderer _spriteRendererOfCurrentNote;

	private AudioSource _audioSource;
	
	private int nextNoteToPlay = 0;
	private bool _isPlaying;
	

	// Use this for initialization
	void Start ()
	{
		if (_noteConfiguration == null || _noteConfiguration.Length == 0)
		{
			// TODO init ghost with random note
		}
		_audioSource = GetComponent<AudioSource>();
		
		_currentPlayingNote = new GameObject();
		
		_currentPlayingNote.SetActive(false);
		var transform = _currentPlayingNote.transform;
		transform.SetParent(this.transform);
		
		transform.position = new Vector2(0, -100);
		_spriteRendererOfCurrentNote = _currentPlayingNote.AddComponent<SpriteRenderer>();
		_spriteRendererOfCurrentNote.sprite = _noteConfiguration[nextNoteToPlay].Sprite;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void FixedUpdate()
	{
		if (_isPlaying && _audioSource.isPlaying == false)
		{
			_spriteRendererOfCurrentNote.sprite = _noteConfiguration[nextNoteToPlay].Sprite;
			_audioSource.PlayOneShot(_noteConfiguration[nextNoteToPlay++].Note);
			if (nextNoteToPlay >= _noteConfiguration.Length)
			{
				nextNoteToPlay = 0;
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if ("Player".Equals(other.gameObject.tag))
		{
			_currentPlayingNote.SetActive(true);
			_isPlaying = true;
			nextNoteToPlay = 0;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if ("Player".Equals(other.gameObject.tag))
		{
			_currentPlayingNote.SetActive(false);
			_isPlaying = false;
		}
	}
}
