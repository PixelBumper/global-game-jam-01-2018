using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Vector3 _offset;
    private GameObject _player;

    private void Start ()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
	}
	
	private void FixedUpdate ()
    {
        var playerPosition = _player.transform.position;
        transform.position = playerPosition + _offset;
    }
}
