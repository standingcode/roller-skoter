using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRayCaster : MonoBehaviour
{
	[SerializeField]
	private LayerMask layerMask;

	[SerializeField]
	private Transform rayCastOrigin;

	private static RaycastHit2D hit;
	public static RaycastHit2D Hit { get => hit; }

	private static bool colliderWasNull = true;
	public static bool ColliderWasNull { get => colliderWasNull; }

	private void Start()
	{
		StartCoroutine(ConstantRayCastingRoutine());
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private IEnumerator ConstantRayCastingRoutine()
	{
		while (true)
		{
			ConstantRayCasting();
			yield return null;
		}
	}

	public void ConstantRayCasting()
	{
		hit = Physics2D.Raycast(rayCastOrigin.position, Vector2.down, 10, layerMask);
		colliderWasNull = hit.collider == null;
	}
}
