using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScrolling : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	public float multiplier = 1;

	public void Update()
	{
		spriteRenderer.material.mainTextureOffset = new Vector2(
		spriteRenderer.material.mainTextureOffset.x + Time.deltaTime * multiplier,
		spriteRenderer.material.mainTextureOffset.y + Time.deltaTime * multiplier);
	}
}
