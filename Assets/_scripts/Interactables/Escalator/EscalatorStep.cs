using UnityEngine;

public class EscalatorStep : MonoBehaviour
{
	public Escalator Escalator { get; set; }

	[SerializeField]
	private BoxCollider2D shortCollider, longCollider;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.GetContact(0).point.y > transform.position.y)
			Escalator.SetPlayerAsChildOfStep(collision.transform.root, transform);
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
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
}
