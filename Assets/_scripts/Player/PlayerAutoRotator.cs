using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerAutoRotator : MonoBehaviour
{
	[SerializeField]
	private float fixRotationSpeed = 150f;

	[SerializeField]
	private LayerMask layerMask;

	[SerializeField]
	PlayerAnimationControl playerAnimationControl;

	public Transform RaycastOrigin;

	private Vector3 newEulerAngles = Vector3.zero;

	[SerializeField]
	private float rotatingZTarget = 0;

	private float degreesOfRotationDifference;

	private void Update()
	{
		DetermineRotation();
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{

	}

	private void OnCollisionExit2D(Collision2D collision)
	{

	}
	public void DetermineRotation()
	{
		if (PlayerControl.Hit.collider == null)
		{
			rotatingZTarget = 0;
		}
		else
		{
			if (PlayerControl.Hit.collider.tag.Equals("UseColliderMainRotation"))
			{
				rotatingZTarget = PlayerControl.Hit.transform.eulerAngles.z;
			}
			else
			{
				rotatingZTarget = Quaternion.FromToRotation(Vector3.up, PlayerControl.Hit.normal).eulerAngles.z;
			}
		}

		CheckDirectionAndCallRotate();
	}

	public void CheckDirectionAndCallRotate()
	{
		if (this.transform.eulerAngles.z != rotatingZTarget)
		{
			degreesOfRotationDifference = this.transform.eulerAngles.z - rotatingZTarget;

			var amountToRotateThisFrame = Time.deltaTime * fixRotationSpeed;

			if (degreesOfRotationDifference > 0)
			{
				if (degreesOfRotationDifference > 180)
				{
					degreesOfRotationDifference = 360 - degreesOfRotationDifference;

					// Rotate right							
					if (RotatePlayerZTowardsTarget(amountToRotateThisFrame))
					{
						return;
					}

				}

				// Rotate left							
				if (RotatePlayerZTowardsTarget(-amountToRotateThisFrame))
				{
					return;
				}


			}
			else
			{
				degreesOfRotationDifference = Mathf.Abs(degreesOfRotationDifference);

				if (degreesOfRotationDifference > 180)
				{
					degreesOfRotationDifference = 360 - degreesOfRotationDifference;

					// Rotate left							
					if (RotatePlayerZTowardsTarget(-amountToRotateThisFrame))
					{
						return;
					}
				}

				// Rotate right							
				if (RotatePlayerZTowardsTarget(amountToRotateThisFrame))
				{
					return;
				}
			}
		}
	}

	public bool RotatePlayerZTowardsTarget(float amountToRotate)
	{
		// If we would overshoot the target or hit it dead on, just set it
		if (Mathf.Abs(amountToRotate) >= degreesOfRotationDifference)
		{
			newEulerAngles.x = this.transform.eulerAngles.x;
			newEulerAngles.y = this.transform.eulerAngles.y;
			newEulerAngles.z = rotatingZTarget;

			this.transform.eulerAngles = newEulerAngles;

			return true;
		}

		// Just do some rotation then
		newEulerAngles.x = this.transform.eulerAngles.x;
		newEulerAngles.y = this.transform.eulerAngles.y;
		newEulerAngles.z = this.transform.eulerAngles.z + amountToRotate;

		this.transform.eulerAngles = newEulerAngles;

		return false;
	}

	//private void OnDrawGizmos()
	//{
	//	Gizmos.color = Color.red;
	//	Gizmos.DrawRay(RaycastOrigin.position, Vector2.down * 30);
	//}
}
