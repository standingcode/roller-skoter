using System.Collections.Generic;
using UnityEngine;

public class EscalatorStep : MonoBehaviour
{
	public Escalator Escalator { get; set; }

	[SerializeField]
	private BoxCollider2D shortCollider;

	private int playerCollisionCount = 0;

	private string playerTag = "Player";

	private Transform playerRootTransform => PlayerReferences.Instance.transform;

	public void MoveStep(Vector3 vectorToMove)
	{
		transform.position += vectorToMove;
	}

	public void SetPositionOfStep(Vector3 position)
	{
		transform.position = position;
	}

	public void ObjectCollidedWithTopOfStep(Transform otherTransform)
	{
		if (otherTransform.tag == playerTag)
		{
			if (playerCollisionCount == 0)
			{
				SetThisStepAsParent();
			}

			playerCollisionCount++;
		}
	}

	public void ObjectLeftStep(Transform otherTransform)
	{
		if (otherTransform.tag == playerTag)
		{
			playerCollisionCount--;

			if (playerCollisionCount == 0)
			{
				NullPlayerParent();
			}
		}
	}

	public void ActivateCollider()
	{
		shortCollider.gameObject.SetActive(true);
	}

	public void DeactivateCollider()
	{
		RemovePlayerFromStep();
		shortCollider.gameObject.SetActive(false);
	}

	public void RemovePlayerFromStep()
	{
		// Need to think later about other stuff being on the step
		NullPlayerParent();
	}

	public void SetThisStepAsParent()
	{
		playerRootTransform.SetParent(this.transform);
	}

	public void NullPlayerParent()
	{
		if (playerRootTransform.parent != this.transform)
			return;

		playerRootTransform.SetParent(null);
		playerCollisionCount = 0;
	}

	//private Vector3 contactPoint;
	//private void OnDrawGizmos()
	//{
	//	Gizmos.color = Color.red;
	//	Gizmos.DrawSphere(contactPoint, 0.1f);
	//}
}
