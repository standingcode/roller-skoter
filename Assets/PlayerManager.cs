using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	[SerializeField]
	private PlayerControl playerControl;

	[SerializeField]
	private Rigidbody2D mainRigidbody;

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
		mainRigidbody.transform.position = transform.position;
		mainRigidbody.transform.rotation = transform.rotation;
	}
}
