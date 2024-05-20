using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerManager : MonoBehaviour
{
	[SerializeField]
	private PlayerControl playerControl;

	[SerializeField]
	private Rigidbody2D mainRigidbody;

	[SerializeField]
	private Transform playerScalableRoot;

	[SerializeField]
	private SortingGroup sortingGroup;

	[SerializeField]
	private float playerBackgroundScale = 0.4f;

	private Vector3 playerOriginalScale;

	private Vector2 startPosition;
	private Quaternion startRotation;


	// Start is called before the first frame update
	void Start()
	{
		startPosition = transform.position;
		startRotation = transform.rotation;
	}

	public void ResetPlayerToStart()
	{
		playerControl.ZeroAllForcesAndSpeed();
		mainRigidbody.transform.position = startPosition;
		mainRigidbody.transform.rotation = startRotation;
	}

	[ContextMenu("MovePlayerToBackgroundLayer")]
	public void MovePlayerToBackgroundLayer()
	{
		playerOriginalScale = playerScalableRoot.localScale;
		sortingGroup.sortingLayerID = SortingLayer.NameToID("PlayerBackgroundLayer");
		playerScalableRoot.localScale = new Vector3(playerBackgroundScale, playerBackgroundScale, 1);
	}

	[ContextMenu("MovePlayerToForegroundLayer")]
	public void MovePlayerToForegroundLayer()
	{
		sortingGroup.sortingLayerID = SortingLayer.NameToID("Player");
		playerScalableRoot.localScale = playerOriginalScale;
	}
}
