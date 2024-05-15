using UnityEngine;

public class ConstantRayCasting : MonoBehaviour
{
	private RaycastHit2D hit;
	public RaycastHit2D Hit { get => hit; }

	private RaycastHit2D hit2;
	public RaycastHit2D Hit2 { get => hit2; }

	[SerializeField]
	private Transform raycastOrigin;
	public Transform RaycastOrigin => raycastOrigin;

	[SerializeField]
	private Transform raycastOrigin2;
	public Transform RaycastOrigin2 => raycastOrigin2;

	[SerializeField]
	private LayerMask floorLayerMask;

	private void Update()
	{
		hit = Physics2D.Raycast(raycastOrigin.position, -transform.up, 30, floorLayerMask);
		hit2 = Physics2D.Raycast(raycastOrigin2.position, -transform.up, 30, floorLayerMask);
	}
}
