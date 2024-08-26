using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
	[SerializeField]
	private float cameraSpeed;

	[SerializeField]
	private Transform playerTransform;


	void Start()
	{

	}

	float targetXPosition;
	float targetYPosition;
	void LateUpdate()
	{
		targetXPosition = playerTransform.position.x;
		targetYPosition = playerTransform.position.y;

		targetXPosition = Mathf.Max(targetXPosition, 0);
		targetYPosition = Mathf.Max(targetYPosition, 0);

		this.transform.position = new Vector3(targetXPosition, targetYPosition, this.transform.position.z);
	}
}
