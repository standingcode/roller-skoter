using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationControl : MonoBehaviour
{
	[SerializeField]
	private Animator animator;

	[SerializeField]
	private string skateAnimationName, jumpAnimationName, landAnimationName, idleAnimationName;

	[SerializeField]
	private LayerMask layerMask;

	[SerializeField]
	private float heightOfFullJumpAnimation = 2f;

	[SerializeField]
	private float lowThresholdForJumpAnimation = 0.024f;

	// Update is called once per frame
	public void PlaySkateAnimation()
	{
		animator.SetBool("IsSkating", true);
	}

	public void PlayIdleAnimation()
	{
		animator.SetBool("IsSkating", false);
	}

	public void PlayJumpAnimation(float position)
	{
		Debug.Log($"Jumping animation position: {position}");
	}


	//// Auto jump animation

	//float jumpAnimationRatio = 0f;
	//private void DetermineJumpAnimation()
	//{
	//	//Debug.Log($"Raycast distance is: {hit.distance}");

	//	if (hit.distance < lowThresholdForJumpAnimation)
	//		return;

	//	jumpAnimationRatio = Mathf.Clamp01(hit.distance / heightOfFullJumpAnimation);
	//	playerAnimationControl.PlayJumpAnimation(jumpAnimationRatio);
	//}

}
