using System;
using System.Collections;
using System.Collections.Generic;
using GGJ.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGJ.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class Choreographer : MonoBehaviour
    {
        private readonly WaitForSeconds _longRestartWait = new WaitForSeconds(1.5f);
        private readonly WaitForSeconds _symbolWait = new WaitForSeconds(0.5f);
        private readonly WaitForSeconds _errorWait = new WaitForSeconds(0.15f);

        [SerializeField] private DancingSymbol[] _dancingSymbols;
        [SerializeField] private string _nextSceneToLoad;
        [SerializeField] private AudioClip _audioClip;

        private AudioSource _audioSource;
        private Coroutine _hintCoroutine;
        private Coroutine _waitCoroutine;
        private Coroutine _errorCoroutine;
        private int _nextCorrectIndex = 0;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _hintCoroutine = StartCoroutine(PlayHintsInSequence());
            InputController.Instance.FireChanges.Subscribe(firePressed =>
            {
                if (_waitCoroutine != null)
                {
                    StopCoroutine(_waitCoroutine);
                    _waitCoroutine = null;
                }

                if (_hintCoroutine != null)
                {
                    StopCoroutine(_hintCoroutine);
                    _hintCoroutine = null;
                }


                if (_nextCorrectIndex >= _dancingSymbols.Length)
                {
                    return;
                }

                var indexOfSymbol = -1;
                for (int i = 0; i < _dancingSymbols.Length; i++)
                {
                    DancingSymbol currentDancingSymbol = _dancingSymbols[i];
                    if (firePressed.ToString().Equals(currentDancingSymbol.SingleNoteConfig.button))
                    {
                        indexOfSymbol = i;
                        break;
                    }
                }

                if (indexOfSymbol == _nextCorrectIndex)
                {
                    _nextCorrectIndex++;
                }
                else
                {
                    _nextCorrectIndex = 0;
                    PlayErrorSoundDelayed();
                }

                if (_nextCorrectIndex >= _dancingSymbols.Length)
                {
                    RunNextGame();
                    return;
                }

                _waitCoroutine = StartCoroutine(RestartHints());
            });
        }

        private void PlayErrorSoundDelayed()
        {
            if (_errorCoroutine != null)
            {
                StopCoroutine(_errorCoroutine);
                _errorCoroutine = null;
            }
            _errorCoroutine = StartCoroutine(PlayErrorSoundEnum());
        }

        private IEnumerator PlayErrorSoundEnum()
        {
            yield return _errorWait;
            _audioSource.PlayOneShot(_audioClip);
        }

        private void RunNextGame()
        {
            StartCoroutine(RunNextGameDelayed());
        }

        private IEnumerator RunNextGameDelayed()
        {
            for (int i = 0; i < _dancingSymbols.Length; i++)
            {
                var dancingSymbol = _dancingSymbols[i];
                dancingSymbol.Disappear();
                yield return _errorWait;
            }
            SceneManager.LoadScene(_nextSceneToLoad);
        }

        private IEnumerator PlayHintsInSequence()
        {
            while (true)
            {
                yield return _symbolWait;

                for (int i = 0; i < _dancingSymbols.Length; i++)
                {
                    var dancingSymbol = _dancingSymbols[i];
                    dancingSymbol.PlayNoteAsHint();
                    yield return _symbolWait;
                }

                yield return _symbolWait;
            }
        }

        private IEnumerator RestartHints()
        {
            yield return _longRestartWait;
            _nextCorrectIndex = 0;
            _hintCoroutine = StartCoroutine(PlayHintsInSequence());
        }
    }
}
