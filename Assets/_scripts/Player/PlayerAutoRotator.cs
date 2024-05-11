using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Min rotation speed for on the floor
// Max rotation speed
// The height at which max rotation should be applied (probably max jump height?)

public class PlayerAutoRotator : MonoBehaviour
{
	[SerializeField]
	private float maxFixRotationSpeed = 1000f, minRotationSpeed = 100f;

	[SerializeField]
	private AnimationCurve rotationSpeedCurve;

	[SerializeField]
	private LayerMask layerMask;

	[SerializeField]
	PlayerAnimationControl playerAnimationControl;

	public Transform RaycastOrigin;

	private Vector3 newEulerAngles = Vector3.zero;

	[SerializeField]
	private float rotatingZTarget = 0;

	//public float playerCurrentZRotation;

	private float degreesOfRotationDifference;

	private void Update()
	{
		//playerCurrentZRotation = this.transform.eulerAngles.z;
		DetermineRotation();
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	public void DetermineRotation()
	{
		if (ConstantRayCasting.Hit.collider == null)
		{
			rotatingZTarget = 0;
		}
		else
		{
			if (ConstantRayCasting.Hit.collider.tag.Equals("UseColliderMainRotation"))
			{
				rotatingZTarget = ConstantRayCasting.Hit.transform.eulerAngles.z;
			}
			else
			{
				rotatingZTarget = Quaternion.FromToRotation(Vector3.up, ConstantRayCasting.Hit.normal).eulerAngles.z;
			}
		}

		CheckDirectionAndCallRotate();
	}

	float ratioBetweenFloorAndMaxHeight;
	float curveRatio;
	public float GetRotationSpeed()
	{
		// Ratio between height above floor and max height clamped to 0 and 1.
		ratioBetweenFloorAndMaxHeight = Mathf.Clamp01(ConstantRayCasting.Hit.distance / PlayerControl.MaxJumpHeight);

		// Then use the curve to give a non-linear ratio.
		curveRatio = rotationSpeedCurve.Evaluate(ratioBetweenFloorAndMaxHeight);

		// Apply this ratio where 0 would be max rotation speed and 1 would be min rotation speed.
		// max - ((max - min) * ratio)

		return maxFixRotationSpeed - ((maxFixRotationSpeed - minRotationSpeed) * curveRatio);
	}

	public void CheckDirectionAndCallRotate()
	{
		if (this.transform.eulerAngles.z != rotatingZTarget)
		{
			degreesOfRotationDifference = rotatingZTarget - this.transform.eulerAngles.z;

			var amountToRotateThisFrame = Time.deltaTime * GetRotationSpeed();

			// Rotate left
			if ((degreesOfRotationDifference > 0 && degreesOfRotationDifference >= 180)
			|| (degreesOfRotationDifference < 0 && Mathf.Abs(degreesOfRotationDifference) <= 180))
			{
				degreesOfRotationDifference = Mathf.Abs(degreesOfRotationDifference);

				// Use this if over 180 degrees
				if (degreesOfRotationDifference > 180)
					degreesOfRotationDifference = 360 - degreesOfRotationDifference;

				// Rotate left							
				RotatePlayerZTowardsTarget(-amountToRotateThisFrame);
			}
			//Rotate right
			else
			{
				degreesOfRotationDifference = Mathf.Abs(degreesOfRotationDifference);

				// Use this if over 180 degrees
				if (degreesOfRotationDifference > 180)
					degreesOfRotationDifference = 360 - degreesOfRotationDifference;

				// Rotate right							
				RotatePlayerZTowardsTarget(amountToRotateThisFrame);
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
