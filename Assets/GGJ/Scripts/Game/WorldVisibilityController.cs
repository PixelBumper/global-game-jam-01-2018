using System;
using UnityEngine;

public class WorldVisibilityController : MonoBehaviour
{
    [SerializeField] private EWorldStatus _visibleStatus;

    private void Start ()
    {
        var gameManager = FindObjectOfType<GameManager>();
        gameManager.WorldChanges.Subscribe(OnWorldChanges);
    }

    private void OnWorldChanges(EWorldStatus newStatus)
    {
        var shouldBeVisible = _visibleStatus == newStatus;
        if (gameObject.activeSelf != shouldBeVisible)
        {
            gameObject.SetActive(shouldBeVisible);
        }
    }
}
