using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Hierarchy;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
	[SerializeField]
	private float jumpingForce;

	[SerializeField]
	private float skatingForce;

	[SerializeField]
	private Rigidbody2D mainRigidbody;

	[SerializeField]
	private ConstantForce2D constantForce2D;

	[SerializeField]
	private Transform centerOfMass;

	[SerializeField]
	private LayerMask floorLayerMask;

	[SerializeField]
	private Transform spriteTransform;

	[SerializeField]
	private Transform raycastOrigin;

	[SerializeField]
	private float heightToConsiderBeingOffTheGround = 0.04f;

	[SerializeField]
	private float waitForUnJumpTime = 0.15f;

	public static Action PlayerJumped;
	public static Action PlayerLanded;

	private static RaycastHit2D hit;
	public static RaycastHit2D Hit { get => hit; }

	protected bool currentlyFlying;
	public bool CurrentlyFlying
	{
		get => currentlyFlying;
		set => currentlyFlying = value;
	}

	protected CharacterFacingDirection characterCurrentFacingDirection = CharacterFacingDirection.Right;
	public CharacterFacingDirection CharacterCurrentFacingDirection
	{
		get => characterCurrentFacingDirection;
		private set => characterCurrentFacingDirection = value;
	}

	private void Start()
	{
		mainRigidbody.centerOfMass = centerOfMass.localPosition;
		heightToConsiderBeingOffTheGround = raycastOrigin.localPosition.y - centerOfMass.localPosition.y + 0.1f;
	}

	private void Update()
	{
		ConstantRayCasting();
	}

	public void ConstantRayCasting()
	{
		hit = Physics2D.Raycast(raycastOrigin.position, Vector2.down, 30, floorLayerMask);
		CheckForJumpOrFall();
	}

	public void CheckForJumpOrFall()
	{
		// If the ray hits nothing, and CurrentlyFlying is false, then we need to take action
		// If the ray distance is over the threshold and CurrentlyFlying is false, then we need to take action
		if ((hit.collider == null | hit.distance >= heightToConsiderBeingOffTheGround) && CurrentlyFlying == false)
		{
			CurrentlyFlying = true;
			PlayerJumped?.Invoke();

			return;
		}

		// If the ray distance is lower than threshold, and CurrentlyFlying is true, then we need to take action
		if (hit.collider != null && hit.distance < heightToConsiderBeingOffTheGround)
		{
			CurrentlyFlying = false;
			PlayerLanded?.Invoke();
		}
	}

	public void PowerLeft()
	{
		//Debug.Log("Power left");

		TurnCharacter(CharacterFacingDirection.Left);
		constantForce2D.force = new Vector2(-skatingForce, 0);
	}

	public void PowerRight()
	{
		//Debug.Log("Power right");

		TurnCharacter(CharacterFacingDirection.Right);
		constantForce2D.force = new Vector2(skatingForce, 0);
	}

	public void TurnCharacter(CharacterFacingDirection characterFacingDirection)
	{
		if (CharacterCurrentFacingDirection == characterFacingDirection)
		{
			return;
		}

		CharacterCurrentFacingDirection = characterFacingDirection;
		spriteTransform.Rotate(0f, 180f, 0f);
	}

	public void UnPower()
	{
		//Debug.Log("Unpower");

		constantForce2D.force = new Vector2(0, 0);
	}

	public void Jump()
	{
		//Debug.Log("Jump");

		if (CurrentlyFlying)
			return;

		CurrentlyFlying = true;

		mainRigidbody.AddForce(transform.up * jumpingForce, ForceMode2D.Impulse);
	}

	public void UnJump()
	{
		if (!CurrentlyFlying)
			return;

		if (unJumpCoroutine == null && mainRigidbody.velocityY > 0)
		{
			unJumpCoroutine = StartCoroutine(UnJumpCoroutine());
		}
	}

	private Coroutine unJumpCoroutine = null;
	public IEnumerator UnJumpCoroutine()
	{
		yield return new WaitForSeconds(waitForUnJumpTime);

		mainRigidbody.velocityY = 0;
		unJumpCoroutine = null;
	}

	public void ZeroAllForcesAndSpeed()
	{
		constantForce2D.force = new Vector2(0, 0);
		mainRigidbody.velocity = new Vector2(0, 0);
	}
}

public enum CharacterFacingDirection
{
	Left,
	Right
}
