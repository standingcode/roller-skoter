using UnityEngine;

public class ConstantRayCasting : MonoBehaviour
{
	private static RaycastHit2D hit;
	public static RaycastHit2D Hit { get => hit; }

	[SerializeField]
	private Transform raycastOrigin;

	[SerializeField]
	private LayerMask floorLayerMask;

	void Update()
	{
		hit = Physics2D.Raycast(raycastOrigin.position, Vector2.down, 30, floorLayerMask);
	}
}
