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
	private Transform raycastOrigin;

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

	}
}
