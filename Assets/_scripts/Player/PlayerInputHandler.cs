using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[ExecuteAlways]
public class PlayerInputHandler : MonoBehaviour
{
	[SerializeField]
	private PlayerControl playerControl;

	[SerializeField]
	private PlayerManager playerManager;

	[SerializeField]
	private PlayerAnimationControl playerAnimationControl;

	[SerializeField]
	private float leftStickXDeadZone;

	// KEYBOARD INPUT

	public void OnPowerLeft(InputValue inputValue)
	{
		PowerLeft();
	}

	public void OnPowerRight(InputValue inputValue)
	{
		PowerRight();
	}

	public void OnUnPower(InputValue inputValue)
	{
		UnPower();
	}

	public void OnJump(InputValue inputValue)
	{
		Jump();
	}


	// GAMEPAD INPUT

	float leftStickX;
	bool valueIsOnOppositeSideOfZero = false;
	List<float> lastValues = new List<float>();
	int maxLastValues = 5;
	public void OnLeftStickMoved(InputValue inputValue)
	{
		leftStickX = inputValue.Get<float>();

		lastValues.Add(leftStickX);

		if (lastValues.Count > maxLastValues)
			lastValues.RemoveAt(0);

		leftStickX = lastValues.Average();

		if (Mathf.Abs(leftStickX) < leftStickXDeadZone)
		{
			UnPower();
		}
		else if (leftStickX > 0)
		{
			PowerRight();
		}
		else if (leftStickX < 0)
		{
			PowerLeft();
		}
	}

	public void OnJumpButtonPressed(InputValue inputValue)
	{
		Jump();
	}
	public void OnJumpButtonReleased(InputValue inputValue)
	{
		UnJump();
	}

	// SHARED INPUT
	public void OnReset()
	{
		//Debug.Log($"Reset");
		playerManager.ResetPlayerToStart();
	}

	public void PowerLeft()
	{
		//Debug.Log($"Power left");

		playerControl.PowerLeft();
		playerAnimationControl.PlaySkateAnimation();
	}

	private Coroutine powerRightCoroutine = null;
	public void PowerRight()
	{
		//Debug.Log($"Power right");

		playerControl.PowerRight();
		playerAnimationControl.PlaySkateAnimation();
	}

	public void UnPower()
	{
		//Debug.Log($"UnPower");

		playerControl.UnPower();
		playerAnimationControl.PlayIdleAnimation();
	}


	// SHARED METHODS	

	public void Jump()
	{
		//Debug.Log($"Jump");

		playerControl.Jump();
	}

	public void UnJump()
	{
		playerControl.UnJump();
	}
}
