using System.Collections;
using System.Collections.Generic;
using GGJ.Scripts.ScriptableObjects;
using UniRx;
using UnityEngine;
using DG.Tweening;

namespace GGJ.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class DancingSymbol : MonoBehaviour
    {
        [SerializeField] private SingleNoteConfiguration _singleNoteConfiguration;

        [SerializeField] private SpriteRenderer _singleNoteSprite;
        private AudioSource _audioSource;

        public SingleNoteConfiguration SingleNoteConfig
        {
            get
            {
                return _singleNoteConfiguration;
            }
        }

        public void Awake()
        {
            DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
        }

        public void Start()
        {
            _singleNoteSprite.sprite = _singleNoteConfiguration.Sprite;

            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayNoteAsHint()
        {
            _audioSource.PlayOneShot(_singleNoteConfiguration.Note);

            Sequence animationSequence = DOTween.Sequence();
            animationSequence.Append(_singleNoteSprite.transform.DOScale(Vector3.one * 2, 0.2f));
            animationSequence.Append(_singleNoteSprite.transform.DOScale(Vector3.one, 0.5f));
            animationSequence.Play();
        }

        public bool IsPlayingNote()
        {
            return _audioSource.isPlaying;
        }

        public void PlayNoteByPlayer()
        {

        }
    }
}
