using System;
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

	private void Update()
	{
		ConstantRayCasting();
	}

	public void ConstantRayCasting()
	{
		hit = Physics2D.Raycast(rayCastOrigin.position, -transform.up, 30, layerMask);
	}
}
