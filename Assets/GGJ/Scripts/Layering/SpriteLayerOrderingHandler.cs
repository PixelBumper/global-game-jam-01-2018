using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteLayerOrderingHandler : MonoBehaviour {
	private SpriteRenderer _spriteRenderer;

	void Start () {
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void FixedUpdate()
	{
		_spriteRenderer.sortingOrder = (int) (-transform.position.z * 10);
	}
}
