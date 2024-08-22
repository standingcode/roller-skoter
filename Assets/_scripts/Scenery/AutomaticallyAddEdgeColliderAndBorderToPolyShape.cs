using UnityEngine;
using UnityEngine.ProBuilder;

[ExecuteInEditMode]
public class AutomaticallyAddEdgeColliderAndBorderToPolyShape : MonoBehaviour
{
#if UNITY_EDITOR

	public EdgeCollider2D edgeCollider2D;
	public LineRenderer lineRenderer;
	public PolyShape polyShape;

	Vector3[] points;
	void Update()
	{
		if (Application.isPlaying)
			return;

		if (polyShape == null)
			return;

		points = new Vector3[polyShape.controlPoints.Count];

		for (int i = 0; i < polyShape.controlPoints.Count; i++)
		{
			points[i] = new Vector2(polyShape.controlPoints[i].x, polyShape.controlPoints[i].z);
		}

		AddCollider();
		AddBorder();
	}

	private void AddCollider()
	{
		if (edgeCollider2D == null)
			return;

		edgeCollider2D.points = ExtensionMethods.Vector3ArrayToVector2Array(points);
	}

	private void AddBorder()
	{
		if (lineRenderer == null || polyShape == null)
			return;

		lineRenderer.positionCount = points.Length;
		lineRenderer.SetPositions(points);
	}

#endif
}
