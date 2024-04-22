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
	private LayerMask layerMask;

	[SerializeField]
	private Transform spriteTransform;

	[SerializeField]
	private Transform raycastOrigin;

	public static Action PlayerJumped;
	public static Action PlayerLanded;

	protected bool currentlyFlying;
	public bool CurrentlyFlying { get => currentlyFlying; set => currentlyFlying = value; }

	protected CharacterFacingDirection characterCurrentFacingDirection = CharacterFacingDirection.Right;
	public CharacterFacingDirection CharacterCurrentFacingDirection { get => characterCurrentFacingDirection; private set => characterCurrentFacingDirection = value; }

	private void Start()
	{
		mainRigidbody.centerOfMass = centerOfMass.localPosition;
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
		PlayerJumped?.Invoke();

		mainRigidbody.AddForce(transform.up * jumpingForce, ForceMode2D.Impulse);
	}

	RaycastHit2D hit;
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (((1 << collision.gameObject.layer) & layerMask) != 0)
		{
			hit = Physics2D.Raycast(raycastOrigin.position, -transform.up, 10, layerMask);
			if (hit.transform == collision.transform)
			{
				CurrentlyFlying = false;
				PlayerLanded?.Invoke();
			}
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (((1 << collision.gameObject.layer) & layerMask) != 0)
		{
			hit = Physics2D.Raycast(raycastOrigin.position, -transform.up, 10, layerMask);
			if (hit.transform == collision.transform)
			{
				CurrentlyFlying = true;
			}
		}
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
