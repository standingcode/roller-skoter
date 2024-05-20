using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escalator : MonoBehaviour
{
	public bool Play = false;

	[SerializeField]
	private Transform slideSidewaysPoint, spawnPoint, killAimingPoint, aimingPoint, stepsRoot;

	[SerializeField]
	private EscalatorDirection escalatorDirection;

	[SerializeField]
	private float escalatorSpeed;

	private List<Transform> steps = new();

	void Start()
	{
		for (int i = 0; i < stepsRoot.childCount; i++)
		{
			steps.Add(stepsRoot.GetChild(i));
		}
	}

	void FixedUpdate()
	{
		if (!Play)
			return;

		MoveAllStepsExceptFirstStepTowardsAimingPoint();
		MoveFirstStepTowardsKillPosition();
		CheckSecondStep();
	}

	private void CheckSecondStep()
	{
		if (
		(escalatorDirection == EscalatorDirection.UpLeft || escalatorDirection == EscalatorDirection.DownLeft) && (steps[1].position.x <= slideSidewaysPoint.position.x)
		|| (escalatorDirection == EscalatorDirection.UpRight || escalatorDirection == EscalatorDirection.DownRight) && (steps[1].position.x >= slideSidewaysPoint.position.x)
		)
		{
			SpawnAndReorder();
		}
	}

	Transform firstStep;
	private void SpawnAndReorder()
	{
		// Reset second step position to the slide sideways position
		steps[1].position = new Vector3(steps[1].position.x, slideSidewaysPoint.position.y, steps[1].position.z);


		// Do the list swapping stuff, ie move the first step to be the last step

		firstStep = steps[0];
		steps.RemoveAt(0);
		steps.Add(firstStep);
		steps[^1].position = spawnPoint.position;
	}

	private void MoveSecondStepTowardsAimingPoint()
	{
		steps[1].position = Vector3.MoveTowards(steps[1].position, aimingPoint.position, escalatorSpeed * Time.fixedDeltaTime);
	}

	private void MoveFirstStepTowardsKillPosition()
	{
		steps[0].position = Vector3.MoveTowards(steps[0].position, killAimingPoint.position, escalatorSpeed * Time.fixedDeltaTime);
	}

	private void MoveAllStepsExceptFirstStepTowardsAimingPoint()
	{
		for (int i = 1; i < steps.Count; i++)
		{
			steps[i].position = Vector3.MoveTowards(steps[i].position, aimingPoint.position, escalatorSpeed * Time.fixedDeltaTime);
		}
	}

	private void ActivateEscalator()
	{
		Debug.Log("Escalator activated");
	}

	private void DeactivateEscalator()
	{
		Debug.Log("Escalator DEactivated");
	}
}

public enum EscalatorDirection
{
	UpLeft, UpRight, DownLeft, DownRight
}

