using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Vector3 _offset;
    private GameObject _player;
    private Camera _camera;
    private Dictionary<EWorldStatus, Color> _colors;

    private void Start ()
    {
        _colors = new Dictionary<EWorldStatus, Color>
        {
            { EWorldStatus.Ghost, Color.black },
            { EWorldStatus.Living, Color.gray }
        };

        _camera = GetComponent<Camera>();
        _player = GameObject.FindGameObjectWithTag("Player");
        var gameManager = FindObjectOfType<GameManager>();
        gameManager.WorldChanges.Subscribe(OnWorldChanges);
    }

    private void OnWorldChanges(EWorldStatus newStatus)
    {
        _camera.backgroundColor = _colors[newStatus];
    }

    private void FixedUpdate ()
    {
        var playerPosition = _player.transform.position;
        transform.position = playerPosition + _offset;
    }
}
