using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using DG.Tweening;
using System;

namespace GGJ.Scripts
{
    public class InputController : MonoBehaviour
    {
        private static InputController _instance;
        public static InputController Instance
        {
            get
            {
                return _instance;
            }
        }

        public IObservable<EFire> FireChanges;

        void Start()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;

            DontDestroyOnLoad(gameObject);

            FireChanges = Observable.EveryUpdate()
                .SelectMany(
                    Enum.GetValues(typeof(EFire))
                        .Cast<EFire>()
                        .ToObservable()
                        .Where(fire => Input.GetButtonDown(fire.ToString()))
                );
        }
    }
}