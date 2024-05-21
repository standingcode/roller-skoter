using UnityEngine;

public class EscalatorStep : MonoBehaviour
{
	public Escalator Escalator { get; set; }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Escalator.SetPlayerAsChildOfStep(collision.transform.root, transform);
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		Escalator.RemovePlayerAsChildOfStep();
	}
}
