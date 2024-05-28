using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoRotator : MonoBehaviour
{
	[SerializeField]
	private float maxFixRotationSpeed = 1000f, minRotationSpeed = 100f;

	[SerializeField]
	private AnimationCurve rotationSpeedCurve;

	[SerializeField]
	PlayerAnimationControl playerAnimationControl;

	private Vector3 newEulerAngles = Vector3.zero;

	[SerializeField]
	private float rotatingZTarget = 0;

	[SerializeField]
	private float maxDistanceToSurfaceToGetRotationFrom = 5f;

	private float degreesOfRotationDifference;

	private void Update()
	{
		DetermineRotation();
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}


	public void DetermineRotation()
	{
		// Set the target rotation as average of hit1 and hit2	

		if (PlayerReferences.Instance.PlayerControl.PlayerHeight > maxDistanceToSurfaceToGetRotationFrom)
			rotatingZTarget = 0;
		else
			rotatingZTarget = (GetAverageForHit(PlayerReferences.Instance.ConstantRayCasting.Hit) + GetAverageForHit(PlayerReferences.Instance.ConstantRayCasting.Hit2)) / 2f;
		//else
		//	rotatingZTarget = GetAverageForHit(PlayerReferences.Instance.ConstantRayCasting.Hit);

		CheckDirectionAndCallRotate();
	}

	private float eulerAngleZ;
	private float GetAverageForHit(RaycastHit2D hit)
	{
		if (hit.collider == null)
		{
			return 0f;
		}

		if (hit.collider.tag.Equals("UseColliderMainRotation"))
		{
			return hit.transform.eulerAngles.z;
		}

		eulerAngleZ = Quaternion.FromToRotation(Vector3.up, hit.normal).eulerAngles.z;

		eulerAngleZ = eulerAngleZ > 180 ? eulerAngleZ - 360 : eulerAngleZ;

		return eulerAngleZ;
	}

	float ratioBetweenFloorAndMaxHeight;
	float curveRatio;
	public float GetRotationSpeed()
	{
		// Ratio between height above floor and max height clamped to 0 and 1.
		ratioBetweenFloorAndMaxHeight = Mathf.Clamp01(PlayerReferences.Instance.PlayerControl.PlayerHeight / PlayerControl.MaxJumpHeight);

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
