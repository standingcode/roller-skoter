using System.Collections.Generic;
using System.Linq;
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
	private LayerMask floorLayerMask = 0;

	public List<int> LayersToPutBackIntoMaskWhenReturningToForeground;

	private void Start()
	{
		PlayerBackgroundForegroundController.BackgroundModeActivated += SetTemporaryLayerMaskWhenGoingIntoBackgroundMode;
		PlayerBackgroundForegroundController.ForegroundModeActivated += RemoveTemporaryLayerMaskWhenGoingIntoForegroundMode;
	}

	private void OnDestroy()
	{
		PlayerBackgroundForegroundController.BackgroundModeActivated -= SetTemporaryLayerMaskWhenGoingIntoBackgroundMode;
		PlayerBackgroundForegroundController.ForegroundModeActivated -= RemoveTemporaryLayerMaskWhenGoingIntoForegroundMode;

	}

	private void Update()
	{
		hit = Physics2D.Raycast(raycastOrigin.position, -transform.up, 30, floorLayerMask);
		hit2 = Physics2D.Raycast(raycastOrigin2.position, -transform.up, 30, floorLayerMask);
	}

	private void SetTemporaryLayerMaskWhenGoingIntoBackgroundMode(LayerMask layerMask)
	{
		int[] layerIndexes1 = Enumerable.Range(0, 32).Where(index => layerMask == (layerMask | (1 << index))).ToArray();

		LayersToPutBackIntoMaskWhenReturningToForeground = new();

		foreach (var layerIndex in layerIndexes1)
		{
			if ((floorLayerMask & (1 << layerIndex)) != 0)
			{
				floorLayerMask &= ~(1 << layerIndex);
				LayersToPutBackIntoMaskWhenReturningToForeground.Add(layerIndex);
			}
		}
	}

	private void RemoveTemporaryLayerMaskWhenGoingIntoForegroundMode()
	{
		foreach (var layerIndex in LayersToPutBackIntoMaskWhenReturningToForeground)
		{
			floorLayerMask |= (1 << layerIndex);
		}
	}
}
