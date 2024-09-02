using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
	[SerializeField]
	private Transform playerTransform;

	[SerializeField]
	private float smoothSpeed = 0.5f;

	float targetXPosition;
	float targetYPosition;
	void Update()
	{
		targetXPosition = playerTransform.position.x;
		targetYPosition = playerTransform.position.y;

		targetXPosition = Mathf.Max(targetXPosition, 0);
		targetYPosition = Mathf.Max(targetYPosition, 0);

		//this.transform.position = new Vector3(targetXPosition, targetYPosition, this.transform.position.z);


		this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(targetXPosition, targetYPosition, this.transform.position.z), smoothSpeed);
	}
}
