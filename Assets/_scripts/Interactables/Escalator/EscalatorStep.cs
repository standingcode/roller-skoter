using System.Collections.Generic;
using UnityEngine;

public class EscalatorStep : MonoBehaviour
{
	public Escalator Escalator { get; set; }

	[SerializeField]
	private BoxCollider2D shortCollider;

	private int playerCollisionsWithStep = new();

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.GetContact(0).normal.x > (Vector2.left + Vector2.up).x && collision.GetContact(0).normal.y < (Vector2.left + Vector2.up).y &&
			collision.GetContact(0).normal.x < (Vector2.right + Vector2.up).x && collision.GetContact(0).normal.y < (Vector2.right + Vector2.up).y)
		{
			//contactPoint = collision.GetContact(0).point;

			if (playerCollisionsWithStep == 0)
				Escalator.SetPlayerAsChildOfStep(collision.transform.root, transform);

			playerCollisionsWithStep++; ;
		}
		else
		{
			playerCollisionsWithStep = 0;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		playerCollisionsWithStep--;

		if (playerCollisionsWithStep == 0)
			Escalator.RemovePlayerAsChildOfStep(transform);
	}

	//private Vector3 contactPoint;
	//private void OnDrawGizmos()
	//{
	//	Gizmos.color = Color.red;
	//	Gizmos.DrawSphere(contactPoint, 0.1f);
	//}
}
