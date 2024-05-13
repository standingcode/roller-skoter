using System;
using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
	[SerializeField]
	private float jumpingForce;

	[SerializeField]
	private float skatingForce;

	[SerializeField]
	private float maxSpeed = 2f;

	[SerializeField]
	private Rigidbody2D mainRigidbody;

	[SerializeField]
	private ConstantForce2D constantForce2D;

	[SerializeField]
	private Transform centerOfMass;

	[SerializeField]
	private Transform spriteTransform;

	[SerializeField]
	private float heightToConsiderBeingOffTheGround = 0.04f;

	[SerializeField]
	private float waitForUnJumpTime = 0.15f;

	[SerializeField]
	private static float maxJumpHeight;
	public static float MaxJumpHeight { get => maxJumpHeight; }

	[SerializeField]
	private float inAirForceReduceMultiplier = 0.5f;

	public static Action PlayerJumped;
	public static Action PlayerLanded;

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
		maxJumpHeight = CalculateJumpHeight();
	}

	private void Update()
	{
		CheckForJumpOrFall();
		CapMaxSpeed();
	}

	public void CapMaxSpeed()
	{
		if (Mathf.Abs(mainRigidbody.velocityX) >= maxSpeed)
		{
			SetForce(0);
		}
		else
		{
			SetForce(forceSetByController);
		}
	}

	public void CheckForJumpOrFall()
	{
		// If the ray hits nothing, and CurrentlyFlying is false, then we need to take action
		// If the ray distance is over the threshold and CurrentlyFlying is false, then we need to take action
		if (
		(ConstantRayCasting.Hit.collider == null || ConstantRayCasting.Hit.distance >= heightToConsiderBeingOffTheGround)
		&& CurrentlyFlying == false
		)
		{
			CurrentlyFlying = true;
			PlayerJumped?.Invoke();

			return;
		}

		// If the ray distance is lower than threshold, and CurrentlyFlying is true, then we need to take action
		if (ConstantRayCasting.Hit.collider != null && ConstantRayCasting.Hit.distance < heightToConsiderBeingOffTheGround)
		{
			CurrentlyFlying = false;
			PlayerLanded?.Invoke();
		}
	}


	private float forceSetByController;
	public void PowerLeft()
	{
		//Debug.Log("Power left");

		TurnCharacter(CharacterFacingDirection.Left);
		forceSetByController = -skatingForce * (CurrentlyFlying ? inAirForceReduceMultiplier : 1);
	}

	public void PowerRight()
	{
		//Debug.Log("Power right");

		TurnCharacter(CharacterFacingDirection.Right);
		forceSetByController = skatingForce * (CurrentlyFlying ? inAirForceReduceMultiplier : 1);

	}

	private Vector2 forceVector;
	public void SetForce(float xForce, float yForce = 0)
	{
		Debug.Log($"SetForce: {xForce}");

		forceVector.x = xForce;
		forceVector.y = yForce;
		constantForce2D.relativeForce = forceVector;
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

		forceSetByController = 0;

		SetForce(forceSetByController);
	}

	public void Jump()
	{
		//Debug.Log("Jump");

		if (CurrentlyFlying)
			return;

		CurrentlyFlying = true;

		mainRigidbody.AddForce(Vector2.up * jumpingForce, ForceMode2D.Impulse);
	}

	Vector2 lineStart;
	Vector2 lineEnd;
	float CalculateJumpHeight()
	{
		float g = mainRigidbody.gravityScale * Physics2D.gravity.magnitude;
		float v0 = jumpingForce / mainRigidbody.mass; // converts the jumpForce to an initial velocity
		return (v0 * v0) / (2 * g);
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
