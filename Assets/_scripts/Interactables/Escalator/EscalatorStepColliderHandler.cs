using UnityEngine;

public class EscalatorStepColliderHandler : MonoBehaviour
{

	[SerializeField] private EscalatorStep step;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!gameObject.activeSelf)
			return;

		if (collision.GetContact(0).normal.x > (Vector2.left + Vector2.up).x && collision.GetContact(0).normal.y < (Vector2.left + Vector2.up).y &&
			collision.GetContact(0).normal.x < (Vector2.right + Vector2.up).x && collision.GetContact(0).normal.y < (Vector2.right + Vector2.up).y)
		{
			step.ObjectCollidedWithTopOfStep(collision.transform);
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (!gameObject.activeSelf)
			return;

		step.ObjectLeftStep(collision.transform);
	}
}
