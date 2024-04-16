using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoRotator : MonoBehaviour
{
	[SerializeField]
	private float fixRotationSpeed = 1f;

	[SerializeField]
	private Transform raycastOrigin;

	[SerializeField]
	private LayerMask layerMask;

	private Vector3 newEulerAngles = Vector3.zero;
	private RaycastHit2D hit;
	private float rotatingZTarget = 0;
	private IEnumerator CheckDirectionAndCallRotateCoroutine = null;
	private float degreesOfRotationDifference;

	private void Start()
	{
		StartCoroutine(KeepCheckingZRotation());
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private IEnumerator KeepCheckingZRotation()
	{
		while (true)
		{
			//Debug.Log("Checking z rotation");
			CheckPlayerZRotation();
			yield return null;
		}
	}

	public void CheckPlayerZRotation()
	{
		hit = Physics2D.Raycast(raycastOrigin.position, Vector2.down, 10, layerMask);
		if (hit.collider != null)
		{
			rotatingZTarget = hit.transform.eulerAngles.z;

			if (CheckDirectionAndCallRotateCoroutine == null)
			{
				CheckDirectionAndCallRotateCoroutine = CheckDirectionAndCallRotate();
				StartCoroutine(CheckDirectionAndCallRotateCoroutine);
			}
		}
	}

	public IEnumerator CheckDirectionAndCallRotate()
	{
		while (rotatingZTarget != this.transform.eulerAngles.z)
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
						CheckDirectionAndCallRotateCoroutine = null;
						yield break;
					}

					yield return null;
				}

				// Rotate left							
				if (RotatePlayerZTowardsTarget(-amountToRotateThisFrame))
				{
					CheckDirectionAndCallRotateCoroutine = null;
					yield break;
				}

				yield return null;
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
						CheckDirectionAndCallRotateCoroutine = null;
						yield break;
					}

					yield return null;
				}

				// Rotate right							
				if (RotatePlayerZTowardsTarget(amountToRotateThisFrame))
				{
					CheckDirectionAndCallRotateCoroutine = null;
					yield break;
				}

				yield return null;
			}
		}

		CheckDirectionAndCallRotateCoroutine = null;
		yield return null;
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
}
