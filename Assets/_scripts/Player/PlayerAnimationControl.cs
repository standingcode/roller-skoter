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

	private bool isJumping = false;

	private void Start()
	{
		PlayerControl.PlayerJumped += PlayerJumped;
		PlayerControl.PlayerLanded += PlayerLanded;
	}

	private void OnDestroy()
	{
		PlayerControl.PlayerJumped -= PlayerJumped;
		PlayerControl.PlayerLanded -= PlayerLanded;
		StopAllCoroutines();
	}

	public void PlayerJumped()
	{
		if (!this.gameObject.activeSelf)
			return;

		isJumping = true;
		animator.SetBool("IsJumping", true);
		StartCoroutine(DetermineJumpAnimation());
	}
	public void PlayerLanded()
	{
		isJumping = false;
		animator.SetBool("IsJumping", false);
	}
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
		//Debug.Log($"Jumping animation position: {position}");
		animator.SetFloat("JumpPosition", position);
		animator.Play("jump");
	}


	// Auto jump animation

	float jumpAnimationRatio = 0f;
	private IEnumerator DetermineJumpAnimation()
	{
		while (isJumping)
		{
			//Debug.Log($"Raycast distance is: {hit.distance}");		

			//if (PlayerControl.Hit.distance < lowThresholdForJumpAnimation)
			//	yield return null;

			if (PlayerControl.Hit.collider != null)
				jumpAnimationRatio = PlayerControl.Hit.distance / heightOfFullJumpAnimation;
			//else
			//	jumpAnimationRatio = 1;

			PlayJumpAnimation(jumpAnimationRatio);

			yield return null;
		}

		yield break;
	}
}
