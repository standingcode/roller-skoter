using System.Collections.Generic;
using UnityEngine;

public class EscalatorStep : MonoBehaviour
{
	public Escalator Escalator { get; set; }

	[SerializeField]
	private BoxCollider2D shortCollider, longCollider;

	private int playerCollisionsWithStep = new();

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.GetContact(0).point.y > transform.position.y)
		{
			if (playerCollisionsWithStep == 0)
				Escalator.SetPlayerAsChildOfStep(collision.transform.root, transform);

			playerCollisionsWithStep++; ;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		playerCollisionsWithStep--;

		if (playerCollisionsWithStep == 0)
			Escalator.RemovePlayerAsChildOfStep(transform);
	}

	public void SetShortCollider()
	{
		shortCollider.enabled = true;
		longCollider.enabled = false;
	}

	public void SetLongCollider()
	{
		shortCollider.enabled = false;
		longCollider.enabled = true;
	}

	public void Test()
	{
		// NO nothing
	}
}
