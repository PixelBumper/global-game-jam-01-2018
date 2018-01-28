using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MessagesPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] _audioClips;

    [SerializeField] private AudioSource _audioSource;

    private int _nextIndexToPlay;

    // Update is called once per frame
    private void Update()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(_audioClips[_nextIndexToPlay]);
            _nextIndexToPlay = (_nextIndexToPlay + 1) % _audioClips.Length;
        }
    }

    private void OnDestroy()
    {
        _audioSource.Stop();
    }
}
