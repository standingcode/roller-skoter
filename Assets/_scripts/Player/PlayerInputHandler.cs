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

	public void OnPowerLeft(InputValue inputValue)
	{
		//Debug.Log($"Power left");
		playerControl.PowerLeft();
		playerAnimationControl.PlaySkateAnimation();
	}

	public void OnPowerRight(InputValue inputValue)
	{
		//Debug.Log($"Power right");
		playerControl.PowerRight();
		playerAnimationControl.PlaySkateAnimation();
	}

	public void OnUnPower(InputValue inputValue)
	{
		//Debug.Log($"UnPower");

		playerControl.UnPower();
		playerAnimationControl.PlayIdleAnimation();
	}

	public void OnJump(InputValue inputValue)
	{
		//Debug.Log($"Jump");

		playerControl.Jump();
		//playerAnimationControl.PlayJumpAnimation();
	}

	public void OnReset()
	{
		//Debug.Log($"Reset");
		playerManager.ResetPlayerToStart();
	}
}
