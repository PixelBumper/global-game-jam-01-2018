using GGJ.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GGJ.Scripts;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SimpleBubbleDisplayer : MonoBehaviour
{
    private readonly WaitForSeconds _waitForSeconds = new WaitForSeconds(1f);

    [SerializeField] private GameObject _speakBubble;
    [SerializeField] private SpriteRenderer _spriteRendererOfCurrentNote;
    [SerializeField] private SingleNoteConfiguration[] _singleNotes;

    private AudioSource _audioSource;

    private Coroutine _removeBubbleCoroutine;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        InputController.Instance.FireChanges.Subscribe(OnPlayerFireAction);
    }

    private void OnPlayerFireAction(EFire firePressed)
    {
        if (this == null)
        {
            return;
        }

        var indexOfSymbol = -1;
        for (int i = 0; i < _singleNotes.Length; i++)
        {
            SingleNoteConfiguration singleNote = _singleNotes[i];
            if (firePressed.ToString().Equals(singleNote.button))
            {
                indexOfSymbol = i;
                break;
            }
        }

        if (indexOfSymbol < 0)
        {
            return;
        }

        _speakBubble.SetActive(true);
        var currentNote = _singleNotes[indexOfSymbol];
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
        _audioSource.PlayOneShot(currentNote.Note);
        _spriteRendererOfCurrentNote.sprite = currentNote.Sprite;

        if (_removeBubbleCoroutine != null)
        {
            StopCoroutine(_removeBubbleCoroutine);
        }
        _removeBubbleCoroutine = StartCoroutine(RemoveBubbleDelayed());

    }

    private IEnumerator RemoveBubbleDelayed()
    {
        yield return _waitForSeconds;
        _speakBubble.SetActive(false);
    }
}