using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Editor : MonoBehaviour
{
	public SpriteRenderer borderSpriteRenderer, parentSpriteRenderer;

	public float paddingX, paddingY;

	// Update is called once per frame
	void Update()
	{
		borderSpriteRenderer.size = new Vector2(parentSpriteRenderer.size.x + paddingX, parentSpriteRenderer.size.y + paddingY);
	}
}
