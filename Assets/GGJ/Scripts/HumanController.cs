using GGJ.Scripts.ScriptableObjects;
using System;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(AudioSource))]
public class HumanController : MonoBehaviour
{
    [SerializeField] private float _mumblingRange;
    [SerializeField] private SingleNoteConfiguration[] _noteConfiguration;
    [SerializeField] private Animator[] _graphics;
    [SerializeField] private AudioClip _mumblingSound;

    private GameObject _currentPlayingNote;
    private AudioSource _audioSource;
    private SphereCollider _sphereCollider;
    private bool _canMumble = true;
    private IDisposable _dispisable;

    // Use this for initialization
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        var graphics = _graphics[UnityEngine.Random.Range(0, _graphics.Length)];
        Instantiate(graphics, transform);
        GetComponent<SphereCollider>().radius = _mumblingRange;
    }

    void OnTriggerEnter(Collider other)
    {
        if ("Player".Equals(other.gameObject.tag))
        {
            if(_canMumble)
            {
                _canMumble = false;

                _dispisable = Observable.Interval(TimeSpan.FromSeconds(_mumblingSound.length))
                    .Subscribe(_ => { }, Debug.LogException, OnMumbleFinished);

                _audioSource.PlayOneShot(_mumblingSound);
            }
        }
    }

    private void OnMumbleFinished()
    {
        _canMumble = true;
    }

    private void OnDestroy()
    {
        _dispisable.Dispose();
    }
}
