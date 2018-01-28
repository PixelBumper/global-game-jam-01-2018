using System.Collections;
using System.Collections.Generic;
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


        public IObservable<EFire> FireChanges { get { return _fireChanges; } }
        private SimpleObservable<EFire> _fireChanges = new SimpleObservable<EFire>();

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (Input.GetButtonDown(EFire.Fire1.ToString()))
            {
                _fireChanges.OnNext(EFire.Fire1);
            }

            if (Input.GetButtonDown(EFire.Fire2.ToString()))
            {
                _fireChanges.OnNext(EFire.Fire2);
            }

            if (Input.GetButtonDown(EFire.Fire3.ToString()))
            {
                _fireChanges.OnNext(EFire.Fire3);
            }

            if (Input.GetButtonDown(EFire.Fire4.ToString()))
            {
                _fireChanges.OnNext(EFire.Fire4);
            }
        }

    }
}
