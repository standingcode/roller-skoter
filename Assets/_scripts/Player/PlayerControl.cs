using System.Collections;
using System.Collections.Generic;
using Unity.Hierarchy;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
	[SerializeField]
	private float jumpingForce;

	[SerializeField]
	private float motorSpeed;

	[SerializeField]
	private Rigidbody2D mainRigidbody;

	[SerializeField]
	private HingeJoint2D frontHingeJoint;

	public void PowerFrontWheel()
	{
		Debug.Log("Power the front wheel");

		var currentMotor = frontHingeJoint.motor;
		currentMotor.motorSpeed = motorSpeed;
		frontHingeJoint.motor = currentMotor;
	}

	public void UnPowerFrontWheel()
	{
		Debug.Log("Unpower the front wheel");

		var currentMotor = frontHingeJoint.motor;
		currentMotor.motorSpeed = 0;
		frontHingeJoint.motor = currentMotor;
	}

	public void Jump()
	{
		Debug.Log("Jump");
		mainRigidbody.AddForce(Vector2.up * jumpingForce, ForceMode2D.Impulse);
	}
}
