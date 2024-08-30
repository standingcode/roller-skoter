using System.Collections.Generic;
using UnityEngine;

public class EscalatorStep : MonoBehaviour
{
	public Escalator Escalator { get; set; }

	[SerializeField]
	private BoxCollider2D shortCollider;

	private List<Transform> collisionObjects = new();

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.GetContact(0).normal.x > (Vector2.left + Vector2.up).x && collision.GetContact(0).normal.y < (Vector2.left + Vector2.up).y &&
			collision.GetContact(0).normal.x < (Vector2.right + Vector2.up).x && collision.GetContact(0).normal.y < (Vector2.right + Vector2.up).y)
		{
			collisionObjects.Add(collision.transform);

			collision.rigidbody!.interpolation = RigidbodyInterpolation2D.None;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		collisionObjects.Remove(collision.transform);
	}

	public void MoveStep(Vector3 vectorToMove)
	{
		transform.position += vectorToMove;

		foreach (Transform t in collisionObjects)
		{
			t.position += vectorToMove;
		}
	}

	public void SetPositionOfStep(Vector3 position)
	{
		Vector3 offset = transform.position - position;

		transform.position = position;

		//foreach (Transform t in collisionObjects)
		//{
		//	t.position += offset;
		//}
	}

	public void ActivateCollider()
	{
		shortCollider.gameObject.SetActive(true);
	}

	public void DeactivateCollider()
	{
		shortCollider.gameObject.SetActive(false);
		RemoveObjectsFromStep();

	}

	public void RemoveObjectsFromStep()
	{
		// Need to decide later if we just find the player and remove only the player,
		// Not other stuff which might be on the step

		foreach (Transform t in collisionObjects)
		{
			t.GetComponent<Rigidbody2D>()!.interpolation = RigidbodyInterpolation2D.Interpolate;
		}

		collisionObjects.Clear();
	}

	//private Vector3 contactPoint;
	//private void OnDrawGizmos()
	//{
	//	Gizmos.color = Color.red;
	//	Gizmos.DrawSphere(contactPoint, 0.1f);
	//}
}
