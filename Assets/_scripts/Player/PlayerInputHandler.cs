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

	public void OnPowerLeft(InputValue inputValue)
	{
		//Debug.Log($"Power left");
		playerControl.PowerLeft();
	}

	public void OnPowerRight(InputValue inputValue)
	{
		//Debug.Log($"Power right");
		playerControl.PowerRight();
	}

	public void OnUnPower(InputValue inputValue)
	{
		//Debug.Log($"UnPower");

		playerControl.UnPower();
	}

	public void OnJump(InputValue inputValue)
	{
		//Debug.Log($"Jump");

		playerControl.Jump();
	}

	public void OnReset()
	{
		//Debug.Log($"Reset");
		playerManager.ResetPlayerToStart();
	}
}
