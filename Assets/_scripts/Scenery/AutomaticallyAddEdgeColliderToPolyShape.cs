using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder;

[ExecuteInEditMode]
public class AutomaticallyAddEdgeColliderToPolyShape : MonoBehaviour
{
	public EdgeCollider2D edgeCollider2D;
	public PolyShape polyShape;


	Vector2[] points;
	void Update()
	{
		points = new Vector2[polyShape.controlPoints.Count];

		for (int i = 0; i < polyShape.controlPoints.Count; i++)
		{
			points[i] = new Vector2(polyShape.controlPoints[i].x, polyShape.controlPoints[i].z);
		}

		edgeCollider2D.points = points;
	}
}
