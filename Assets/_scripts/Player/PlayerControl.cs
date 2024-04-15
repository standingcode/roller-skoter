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
	private float motorSpeed;

	[SerializeField]
	private Rigidbody2D mainRigidbody;

	[SerializeField]
	private ConstantForce2D constantForce2D;

	[SerializeField]
	private HingeJoint2D frontHingeJoint;

	private void Start()
	{
	}

	public void PowerFrontWheel()
	{
		Debug.Log("Power the front wheel");

		var currentMotor = frontHingeJoint.motor;
		currentMotor.motorSpeed = motorSpeed;
		frontHingeJoint.motor = currentMotor;

		//mainRigidbody.velocity = new Vector2(skatingForce, mainRigidbody.velocity.y);

		constantForce2D.force = new Vector2(skatingForce, 0);

	}

	public void UnPowerFrontWheel()
	{
		Debug.Log("Unpower the front wheel");

		var currentMotor = frontHingeJoint.motor;
		currentMotor.motorSpeed = 0;
		frontHingeJoint.motor = currentMotor;

		constantForce2D.force = new Vector2(0, 0);


	}

	public void Jump()
	{
		Debug.Log("Jump");
		mainRigidbody.AddForce(Vector2.up * jumpingForce, ForceMode2D.Impulse);
	}

	public void ZeroAllForcesAndSpeed()
	{
		constantForce2D.force = new Vector2(0, 0);
		mainRigidbody.velocity = new Vector2(0, 0);
	}
}
