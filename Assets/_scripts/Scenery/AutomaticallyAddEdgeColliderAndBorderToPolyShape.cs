using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;

[ExecuteInEditMode]
public class AutomaticallyAddEdgeColliderAndBorderToPolyShape : MonoBehaviour
{
	public EdgeCollider2D edgeCollider2D;
	public LineRenderer lineRenderer;
	public PolyShape polyShape;

	Vector3[] points;
	void Update()
	{
		points = new Vector3[polyShape.controlPoints.Count];

		for (int i = 0; i < polyShape.controlPoints.Count; i++)
		{
			points[i] = new Vector2(polyShape.controlPoints[i].x, polyShape.controlPoints[i].z);
		}

		edgeCollider2D.points = ExtensionMethods.Vector3ArrayToVector2Array(points);
		lineRenderer.positionCount = points.Length;
		lineRenderer.SetPositions(points);
	}
}
