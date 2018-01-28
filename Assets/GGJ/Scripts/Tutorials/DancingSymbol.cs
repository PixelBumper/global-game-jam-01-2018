using System.Collections;
using System.Collections.Generic;
using GGJ.Scripts.ScriptableObjects;
using UnityEngine;
using DG.Tweening;
using System;

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

            // _disposableObserver = InputController.Instance.FireChanges.Subscribe(OnFirePressed);
        }

        private void OnFirePressed(EFire firePressed)
        {
            if (firePressed.ToString().Equals(_singleNoteConfiguration.button))
            {
                _audioSource.PlayOneShot(_singleNoteConfiguration.Note);
                AnimateBump();
            }
        }

        public void PlayNoteAsHint()
        {
            _audioSource.PlayOneShot(_singleNoteConfiguration.Note);

            AnimateBump();
        }

        private void AnimateBump()
        {
            Sequence animationSequence = DOTween.Sequence();
            animationSequence.Append(_singleNoteSprite.transform.DOScale(Vector3.one * 2, 0.2f));
            animationSequence.Append(_singleNoteSprite.transform.DOScale(Vector3.one, 0.5f));
            animationSequence.Play();
        }

        public bool IsPlayingNote()
        {
            return _audioSource.isPlaying;
        }

        internal void Disappear()
        {
            Sequence animationSequence = DOTween.Sequence();
            animationSequence.Append(_singleNoteSprite.transform.DOScale(Vector3.zero, 0.3f));
            animationSequence.Join(_singleNoteSprite.transform.DORotate(Vector3.up * 90, 0.3f));
            animationSequence.Play();
        }
    }
}
