using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AdjustBoxColliderToSpriteSize : MonoBehaviour
{
	public BoxCollider2D boxCollider2D;
	public SpriteRenderer spriteRenderer;
	public float paddingX, paddingY;

	void Update()
	{
		boxCollider2D.size = new Vector2(spriteRenderer.size.x + paddingX, spriteRenderer.size.y + paddingY);
	}
}
