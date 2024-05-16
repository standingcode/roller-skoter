using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.ProBuilder;

[ExecuteInEditMode]
public class AutomaticallyAddBoxColliderAndBorderToPlaneShape : MonoBehaviour
{
	public BoxCollider2D boxCollider2D;
	public ProBuilderMesh spriteShapeMeshData;
	public LineRenderer lineRenderer;


	void Update()
	{
		AddBorder();
		AddCollider();
	}

	private void AddBorder()
	{
		if (spriteShapeMeshData == null || lineRenderer == null)
			return;


		Vector3[] points = new Vector3[5];

		points[0] = new Vector2(spriteShapeMeshData.positions[5].x, spriteShapeMeshData.positions[5].z);
		points[1] = new Vector2(spriteShapeMeshData.positions[15].x, spriteShapeMeshData.positions[15].z);
		points[2] = new Vector2(spriteShapeMeshData.positions[10].x, spriteShapeMeshData.positions[10].z);
		points[3] = new Vector2(spriteShapeMeshData.positions[0].x, spriteShapeMeshData.positions[0].z);
		points[4] = new Vector2(spriteShapeMeshData.positions[5].x, spriteShapeMeshData.positions[5].z);

		lineRenderer.positionCount = points.Length;
		lineRenderer.SetPositions(points);

	}

	private void AddCollider()
	{
		if (boxCollider2D == null)
			return;


		Vector2 size;

		size.x = spriteShapeMeshData.positions[10].x * 2;
		size.y = spriteShapeMeshData.positions[5].z * 2;

		boxCollider2D.size = size;
	}

	//void OnDrawGizmos()
	//{
	//	Gizmos.color = Color.red;

	//	foreach (Vector3 position in points)
	//		Gizmos.DrawCube(position, new Vector2(1f, 1f));
	//}
}
