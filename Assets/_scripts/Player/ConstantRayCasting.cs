using UnityEngine;

public class ConstantRayCasting : MonoBehaviour
{
	private RaycastHit2D hit;
	public RaycastHit2D Hit { get => hit; }

	[SerializeField]
	private Transform raycastOrigin;
	public Transform RaycastOrigin => raycastOrigin;

	[SerializeField]
	private LayerMask floorLayerMask;

	private void Update()
	{
		hit = Physics2D.Raycast(raycastOrigin.position, -transform.up, 30, floorLayerMask);
	}
}
