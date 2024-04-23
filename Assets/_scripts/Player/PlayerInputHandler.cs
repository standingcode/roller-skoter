using System.Collections;
using System.Collections.Generic;
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

	LastXStickInput lastXStickInput = LastXStickInput.UnPower;

	float leftStickX;
	float lastValue = 0;
	bool valueChangedFrom0OrOppositeThingy = false;

	public void OnLeftStickMoved(InputValue inputValue)
	{
		leftStickX = inputValue.Get<float>();

		valueChangedFrom0OrOppositeThingy = (leftStickX > 0 && lastValue < 0) || (leftStickX < 0 && lastValue > 0);

		lastValue = leftStickX;

		if (valueChangedFrom0OrOppositeThingy || lastValue == 0)
		{
			return;
		}


		if (Mathf.Abs(leftStickX) < leftStickXDeadZone)
		{
			lastXStickInput = LastXStickInput.UnPower;
			UnPower();
		}
		else if (leftStickX > 0)
		{
			lastXStickInput = LastXStickInput.Right;
			PowerRight();
		}
		else if (leftStickX < 0)
		{
			Debug.Log($"Left stick x: {leftStickX}");
			PowerLeft();
		}
	}

	private enum LastXStickInput
	{
		Left, Right, UnPower
	}

	private float waitSeconds = 0.02f;

	private Coroutine waitCoroutine = null;
	public IEnumerator Wait()
	{
		float timeToWait = waitSeconds;
		while (timeToWait > 0)
		{
			timeToWait -= Time.deltaTime;
			yield return null;
		}

		waitCoroutine = null;
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
