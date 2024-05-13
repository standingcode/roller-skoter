using UnityEngine;
using UnityEngine.ProBuilder;

[ExecuteInEditMode]
public class AutomaticallyAddBoxColliderToPlaneShape : MonoBehaviour
{
	public BoxCollider2D boxCollider2D;
	public ProBuilderMesh spriteShapeMeshData;

	Vector2 size;
	void Update()
	{
		size.x = spriteShapeMeshData.positions[10].x * 2;
		size.y = spriteShapeMeshData.positions[5].z * 2;

		boxCollider2D.size = size;
	}

	//void OnDrawGizmos()
	//{
	//	Gizmos.color = Color.red;

	//	foreach (Vector3 position in spriteShapeMeshData.positions)
	//		Gizmos.DrawCube(position, new Vector2(1f, 1f));
	//}
}
