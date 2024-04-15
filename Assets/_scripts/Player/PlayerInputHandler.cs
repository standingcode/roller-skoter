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

	public void OnPowerFront(InputValue inputValue)
	{
		Debug.Log($"Power front");

		playerControl.PowerFrontWheel();
	}

	public void OnUnPowerFront(InputValue inputValue)
	{
		Debug.Log($"UnPower front");

		playerControl.UnPowerFrontWheel();
	}

	public void OnJump(InputValue inputValue)
	{
		Debug.Log($"Jump");

		playerControl.Jump();
	}

	public void OnReset()
	{
		Debug.Log($"Reset");
		playerManager.ResetPlayerToStart();
	}
}
