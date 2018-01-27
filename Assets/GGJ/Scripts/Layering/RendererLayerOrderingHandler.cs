using UnityEngine;

public class RendererLayerOrderingHandler : MonoBehaviour {
	private Renderer _renderer;

	void Start ()
	{
		_renderer = GetComponent<Renderer>();
	}

	private void FixedUpdate()
	{
		_renderer.sortingOrder = (int) (-transform.position.z * 10);
	}
}
