using System.Collections;
using System.Collections.Generic;
using GGJ.Scripts.ScriptableObjects;
using UniRx;
using UnityEngine;

namespace GGJ.Scripts
{
    public class Choreographer : MonoBehaviour {

        [SerializeField] private DancingSymbol[] _dancingSymbols;

        private Coroutine _hintCoroutine;
        private Coroutine _waitCoroutine;

        private void Start(){

            _hintCoroutine = StartCoroutine(PlayHintsInSequence());
            InputController.Instance.FireChanges.Subscribe(firePressed => {
				if(_hintCoroutine != null){
                    StopCoroutine(_hintCoroutine);
                    _hintCoroutine = null;
                }
			});
        }

        private IEnumerator PlayHintsInSequence(){
            while(true){
                yield return new WaitForSeconds(0.5f);

                for (int i = 0; i < _dancingSymbols.Length; i++)
                {
                    var dancingSymbol = _dancingSymbols[i];
                    dancingSymbol.PlayNoteAsHint();
                    yield return new WaitForSeconds(0.5f);
                } 
            }
        }
    }
}