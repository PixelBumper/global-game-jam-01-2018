using System.Collections;
using System.Collections.Generic;
using GGJ.Scripts.ScriptableObjects;
using UniRx;
using UnityEngine;

namespace GGJ.Scripts
{
    public class Choreographer : MonoBehaviour {

        [SerializeField] private DancingSymbol[] _dancingSymbols;

        private Coroutine _coroutine;

        private void Start(){

            _coroutine = StartCoroutine(PlayHintsInSequence());
            InputController.Instance.FireChanges.Subscribe(firePressed => {
				if(_coroutine != null){
                    StopCoroutine(_coroutine);
                    _coroutine = null;
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