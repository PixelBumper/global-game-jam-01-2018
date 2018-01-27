using System;
using UniRx;
using UnityEngine;

public class WorldVisibilityController : MonoBehaviour
{
    [SerializeField] private EWorldStatus _visibleStatus;

    private IDisposable _disposable;

    private void Start ()
    {
        var gameManager = FindObjectOfType<GameManager>();
        _disposable = gameManager.WorldChanges.Subscribe(OnWorldChanges);
    }

    private void OnWorldChanges(EWorldStatus newStatus)
    {
        var shouldBeVisible = _visibleStatus == newStatus;
        if (gameObject.activeSelf != shouldBeVisible)
        {
            gameObject.SetActive(shouldBeVisible);
        }
    }

    private void OnDestroy()
    {
        if(_disposable!=null)
        {
            _disposable.Dispose();
        }
    }
}
