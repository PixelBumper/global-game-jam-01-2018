using System.Collections;
using System.Collections.Generic;
using GGJ.Scripts.ScriptableObjects;
using UniRx;
using UnityEngine;

namespace GGJ.Scripts
{
    public class Choreographer : MonoBehaviour
    {
        private readonly WaitForSeconds _longRestartWait = new WaitForSeconds(1.5f);
        private readonly WaitForSeconds _symbolWait = new WaitForSeconds(0.5f);

        [SerializeField] private DancingSymbol[] _dancingSymbols;

        private Coroutine _hintCoroutine;
        private Coroutine _waitCoroutine;

        private void Start()
        {

            _hintCoroutine = StartCoroutine(PlayHintsInSequence());
            InputController.Instance.FireChanges.Subscribe(firePressed =>
            {
                if (_waitCoroutine != null)
                {
                    StopCoroutine(_waitCoroutine);
                    _waitCoroutine = null;
                }
                _waitCoroutine = StartCoroutine(RestartHints());

                if (_hintCoroutine != null)
                {
                    StopCoroutine(_hintCoroutine);
                    _hintCoroutine = null;
                }
            });
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
            }
        }

        private IEnumerator RestartHints()
        {
            yield return _longRestartWait;
            _hintCoroutine = StartCoroutine(PlayHintsInSequence());
        }
    }
}