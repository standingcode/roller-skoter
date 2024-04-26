using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScrolling : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	public float multiplier = 1;

	private Vector2 cameraPreviousPosition;

	public void Start()
	{
		cameraPreviousPosition = Camera.main.transform.position;
	}

	public void Update()
	{
		CheckAndOffsetTexture();
	}


	public void CheckAndOffsetTexture()
	{
		spriteRenderer.material.mainTextureOffset = new Vector2(
		spriteRenderer.material.mainTextureOffset.x + (Camera.main.transform.position.x - cameraPreviousPosition.x) * multiplier,
		spriteRenderer.material.mainTextureOffset.y + (Camera.main.transform.position.y - cameraPreviousPosition.y) * multiplier);

		cameraPreviousPosition = Camera.main.transform.position;
	}
}
