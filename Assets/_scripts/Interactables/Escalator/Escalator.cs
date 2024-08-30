using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Escalator : MonoBehaviour
{
	[SerializeField]
	private bool Play = true;

	[SerializeField]
	private Transform stepsRoot;

	private Vector3 topStepStartPosition, bottomStepStartPosition;

	[SerializeField]
	private BackgroundInteractable backgroundInteractable;

	[SerializeField]
	private float escalatorSpeed;

	private List<EscalatorStep> escalatorSteps = new();
	private List<EscalatorStep> escalatorStepsForEnablingAndDisablingColliders;

	//private Transform stepParent;
	//private Transform playerTransformReference;

	[SerializeField]
	private List<GameObject> gameObjectsToActivateOnBackgroundMode;

	[SerializeField]
	private int numberOfHorizontalSteps = 0;

	private Vector3 horizontalStepsVector, diagonalStepsVectorBottom, diagonalStepsVectorTop;

	private Vector3 diagonalVector = Vector3.up + Vector3.left;

	void Start()
	{
		// Get all the escalator step scripts and add them to a list, whilst also passing them reference to this Escalator script
		escalatorSteps = stepsRoot.transform.GetComponentsInChildren<EscalatorStep>(true).ToList();

		foreach (var escalatorStep in escalatorSteps)
		{
			escalatorStep.Escalator = this;
			escalatorStep.DeactivateCollider();
		}

		// Set the positions needed for movement		
		topStepStartPosition = escalatorSteps[^1].transform.position;
		bottomStepStartPosition = escalatorSteps[0].transform.position;

		horizontalStepsVector = new Vector3(escalatorSteps[0].transform.position.x - escalatorSteps[1].transform.position.x, 0f, 0f);
		diagonalStepsVectorBottom = escalatorSteps[escalatorSteps.Count / 2].transform.position - escalatorSteps[(escalatorSteps.Count / 2) + 1].transform.position;
		diagonalStepsVectorTop = escalatorSteps[(escalatorSteps.Count / 2) + 1].transform.position - escalatorSteps[escalatorSteps.Count / 2].transform.position;

		// Subscribe to the background interactable events
		backgroundInteractable.ForegroundModeActivated += ForegroundModeWasActivated;
		backgroundInteractable.BackgroundModeActivated += BackgroundModeWasActivated;

		foreach (var gameObject in gameObjectsToActivateOnBackgroundMode)
		{
			gameObject.SetActive(false);
		}

		escalatorStepsForEnablingAndDisablingColliders = escalatorSteps.ToList();
	}

	void Update()
	{
		CheckForSpawningAndMoveSteps();
	}

	//public void SetPlayerAsChildOfStep(Transform collidingTransform, Transform stepTransform)
	//{
	//	return;

	//	if (playerTransformReference == null)
	//	{
	//		playerTransformReference = collidingTransform;
	//	}

	//	stepParent = stepTransform;
	//	playerTransformReference.SetParent(stepTransform);
	//}

	//public void RemovePlayerAsChildOfStep(Transform stepTransform = null)
	//{
	//	return;
	//	if (playerTransformReference == null)
	//		return;

	//	if (stepTransform != null && stepTransform != stepParent)
	//		return;

	//	playerTransformReference.SetParent(null);
	//	playerTransformReference = null;
	//}

	private void ForegroundModeWasActivated()
	{
		foreach (var escalatorStep in escalatorStepsForEnablingAndDisablingColliders)
		{
			escalatorStep.DeactivateCollider();
		}

		foreach (var gameObject in gameObjectsToActivateOnBackgroundMode)
		{
			gameObject.SetActive(false);
		}
	}

	private void BackgroundModeWasActivated()
	{
		foreach (var escalatorStep in escalatorStepsForEnablingAndDisablingColliders)
		{
			escalatorStep.ActivateCollider();
		}

		foreach (var gameObject in gameObjectsToActivateOnBackgroundMode)
		{
			gameObject.SetActive(true);
		}
	}

	private Transform firstStep;
	private EscalatorStep firstEscalatorStep;
	private Vector3 nextFramePositionOfCheckStep;
	private void CheckForSpawningAndMoveSteps()
	{
		if (!Play)
			return;

		// Check for spawning and do the swapping including making sure the index 0 step has long collider enabled
		// This involves seeing if the first step would be on or go past position of second step,
		// and passing the difference (which could be 0,0,0).

		nextFramePositionOfCheckStep = escalatorSteps[GetIndexOfSwitchPositionStep()].transform.position + (Vector3.left * escalatorSpeed * Time.deltaTime);
		if (escalatorSpeed >= 0 && nextFramePositionOfCheckStep.x <= (topStepStartPosition.x - horizontalStepsVector.x) ||
			escalatorSpeed < 0 && nextFramePositionOfCheckStep.x >= (bottomStepStartPosition.x + horizontalStepsVector.x))
		{
			DoRespawn();
		}

		MoveMiddleSteps();
		MoveTopAndBottomSteps();
	}

	private void MoveTopAndBottomSteps()
	{
		if (escalatorSpeed > 0)
		{
			// Top steps
			for (int i = escalatorSteps.Count - 1; i >= GetIndexOfInnerTopHorizontalStep(); i--)
			{
				MoveStepSideways(i, topStepStartPosition.y);
			}

			// Bottom steps
			for (int i = 0; i < GetIndexOfInnerBottomHorizontalStep(); i++)
			{
				MoveStepSideways(i, bottomStepStartPosition.y);
			}

			return;
		}

		// Bottom steps
		for (int i = 0; i <= GetIndexOfInnerBottomHorizontalStep(); i++)
		{
			MoveStepSideways(i, bottomStepStartPosition.y);
		}

		// Top steps
		for (int i = escalatorSteps.Count - 1; i > GetIndexOfInnerTopHorizontalStep(); i--)
		{
			MoveStepSideways(i, topStepStartPosition.y);
		}
	}

	private void MoveMiddleSteps()
	{
		if (escalatorSpeed > 0)
		{
			for (int i = numberOfHorizontalSteps - 1; i < GetIndexOfInnerTopHorizontalStep(); i++)
			{
				MoveStepTowardsAimingPoint(i);
			}

			return;
		}

		for (int i = escalatorSteps.Count - numberOfHorizontalSteps; i > GetIndexOfInnerBottomHorizontalStep(); i--)
		{
			MoveStepTowardsAimingPoint(i);
		}
	}

	private Vector3 amountToMoveStepSideways = Vector3.zero;
	private void MoveStepSideways(int index, float verticalPosition)
	{
		amountToMoveStepSideways.x = Vector3.left.x * escalatorSpeed * Time.deltaTime;

		Vector3 amountToMove = new Vector3(
			amountToMoveStepSideways.x,
			verticalPosition - escalatorSteps[index].transform.position.y,
			amountToMoveStepSideways.z);

		escalatorSteps[index].MoveStep(amountToMove);
	}

	private void MoveStepTowardsAimingPoint(int index)
	{
		escalatorSteps[index].MoveStep(diagonalVector * escalatorSpeed * Time.deltaTime);
	}

	private void DoRespawn()
	{
		// Do the list swapping stuff, ie move the first step to be the last step	

		firstEscalatorStep = escalatorSteps[GetIndexOfFirstStep()];

		firstEscalatorStep.RemoveObjectsFromStep();

		escalatorSteps.RemoveAt(GetIndexOfFirstStep());

		if (escalatorSpeed >= 0)
		{
			escalatorSteps.Insert(0, firstEscalatorStep);

			escalatorSteps[0].SetPositionOfStep(escalatorSteps[1].transform.position + horizontalStepsVector);

			escalatorSteps[GetIndexOfInnerBottomHorizontalStep()]
			.SetPositionOfStep(
			escalatorSteps[GetIndexOfInnerBottomHorizontalStep() + 1].transform.position - diagonalStepsVectorTop
				);

		}
		else
		{
			escalatorSteps.Add(firstEscalatorStep);

			escalatorSteps[^1].SetPositionOfStep(escalatorSteps[^2].transform.position - horizontalStepsVector);

			escalatorSteps[GetIndexOfInnerTopHorizontalStep()]
			.SetPositionOfStep(
				escalatorSteps[GetIndexOfInnerTopHorizontalStep() - 1].transform.position + diagonalStepsVectorTop
				);
		}
	}

	private int GetIndexOfFirstStep()
	{
		if (escalatorSpeed >= 0)
			return escalatorSteps.Count - 1;
		else
			return 0;
	}

	private int GetIndexOfSwitchPositionStep()
	{
		if (escalatorSpeed >= 0)
			return escalatorSteps.Count - 1;
		else
			return 0;
	}

	private int GetIndexOfInnerTopHorizontalStep()
	{
		return escalatorSteps.Count - numberOfHorizontalSteps;
	}

	private int GetIndexOfInnerBottomHorizontalStep()
	{
		return numberOfHorizontalSteps - 1;
	}
}