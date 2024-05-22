using UnityEngine;

public class EscalatorStep : MonoBehaviour
{
	public Escalator Escalator { get; set; }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.GetContact(0).point.y > transform.position.y)
			Escalator.SetPlayerAsChildOfStep(collision.transform.root, transform);
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		Escalator.RemovePlayerAsChildOfStep(transform);
	}
}
