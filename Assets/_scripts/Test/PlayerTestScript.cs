using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTestScript : MonoBehaviour
{
	[SerializeField]
	private Rigidbody2D mainRigidbody;

	[SerializeField]
	private float multiplier = 5f;

	[SerializeField]
	private bool useFixedUpdate = false;

	[SerializeField]
	ConstantForce2D constantForce2D;

	bool PowerLeft = false;
	bool PowerRight = false;

	public void OnPowerLeftKeyboard(InputValue inputValue)
	{
		PowerLeft = true;
	}

	public void OnPowerRightKeyboard(InputValue inputValue)
	{
		PowerRight = true;
	}

	public void OnUnPowerKeyboard(InputValue inputValue)
	{
		PowerLeft = PowerRight = false;
	}

	private void Update()
	{
		if (useFixedUpdate)
			return;

		Power();
	}

	private void FixedUpdate()
	{
		if (!useFixedUpdate)
			return;

		Power();
	}

	private void Power()
	{
		if (PowerLeft)
		{
			//transform.position += Vector3.left * Time.deltaTime * multiplier;

			constantForce2D.force = Vector2.left * multiplier;

			return;
		}
		else if (PowerRight)
		{
			//transform.position += Vector3.right * Time.deltaTime * multiplier;

			constantForce2D.force = Vector2.right * multiplier;

			return;
		}

		constantForce2D.force = Vector2.zero;
	}
}
