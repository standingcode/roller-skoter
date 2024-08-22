using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Editor : MonoBehaviour
{
#if UNITY_EDITOR
	public SpriteRenderer borderSpriteRenderer, parentSpriteRenderer;

	public float paddingX, paddingY;

	// Update is called once per frame
	void Update()
	{
		if (Application.isPlaying)
			return;

		borderSpriteRenderer.size = new Vector2(parentSpriteRenderer.size.x + paddingX, parentSpriteRenderer.size.y + paddingY);
	}

#endif
}
