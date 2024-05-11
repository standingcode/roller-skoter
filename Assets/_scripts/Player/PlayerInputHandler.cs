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

	[SerializeField]
	private int amountOfValuesToUseForAveraging = 5;

	// KEYBOARD INPUT

	public void OnPowerLeftKeyboard(InputValue inputValue)
	{
		PowerLeft();
	}

	public void OnPowerRightKeyboard(InputValue inputValue)
	{
		PowerRight();
	}

	public void OnUnPowerKeyboard(InputValue inputValue)
	{
		UnPower();
	}


	// GAMEPAD INPUT

	float currentLeftStickXAxisValue;
	float averagedValueToUseForLeftStickXAxis;
	List<float> lastXAxisValuesFromLeftStick = new List<float>();
	public void OnLeftStickMoved(InputValue inputValue)
	{
		currentLeftStickXAxisValue = inputValue.Get<float>();

		lastXAxisValuesFromLeftStick.Add(currentLeftStickXAxisValue);

		if (lastXAxisValuesFromLeftStick.Count > amountOfValuesToUseForAveraging)
			lastXAxisValuesFromLeftStick.RemoveAt(0);


		if (Mathf.Abs(currentLeftStickXAxisValue) < leftStickXDeadZone)
		{
			UnPower();
			return;
		}

		averagedValueToUseForLeftStickXAxis = lastXAxisValuesFromLeftStick.Average();

		if (averagedValueToUseForLeftStickXAxis > 0)
		{
			PowerRight();
		}
		else if (averagedValueToUseForLeftStickXAxis < 0)
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
		//return;
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
