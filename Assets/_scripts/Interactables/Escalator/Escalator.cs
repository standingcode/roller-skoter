using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Escalator : MonoBehaviour
{
	public bool Play = false;

	[SerializeField]
	private Transform stepsRoot;

	private Vector3 topStepStartPosition, bottomStepStartPosition;

	[SerializeField]
	private BackgroundInteractable backgroundInteractable;

	[SerializeField]
	private float escalatorSpeed;

	private List<Transform> stepsTransforms = new();
	private List<EscalatorStep> escalatorSteps = new();
	private List<EscalatorStep> escalatorStepsForEnablingAndDisablingColliders;

	private Transform stepParent;
	private Transform playerTransformReference;

	[SerializeField]
	private List<GameObject> gameObjectsToActivateOnBackgroundMode;

	[SerializeField]
	private int numberOfHorizontalSteps = 0;

	private Vector3 horizontalStepsVector, diagonalStepsVectorBottom, diagonalStepsVectorTop;

	private Vector3 diagonalVector = Vector3.up + Vector3.left;

	void Start()
	{
		// Get the transform root of each step
		for (int i = 0; i < stepsRoot.childCount; i++)
		{
			stepsTransforms.Add(stepsRoot.GetChild(i));
		}

		// Get all the escalator step scripts and add them to a list, whilst also passing them reference to this Escalator script
		escalatorSteps = stepsRoot.transform.GetComponentsInChildren<EscalatorStep>(true).ToList();

		foreach (var escalatorStep in escalatorSteps)
		{
			escalatorStep.Escalator = this;
			escalatorStep.gameObject.SetActive(false);
		}

		// Set the positions needed for movement		
		topStepStartPosition = stepsTransforms[^1].position;
		bottomStepStartPosition = stepsTransforms[0].position;

		horizontalStepsVector = new Vector3(stepsTransforms[0].position.x - stepsTransforms[1].position.x, 0f, 0f);
		diagonalStepsVectorBottom = stepsTransforms[stepsTransforms.Count / 2].position - stepsTransforms[(stepsTransforms.Count / 2) + 1].position;
		diagonalStepsVectorTop = stepsTransforms[(stepsTransforms.Count / 2) + 1].position - stepsTransforms[stepsTransforms.Count / 2].position;

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
		if (!Play)
			return;

		CheckForSpawningAndMoveSteps();
	}

	public void SetPlayerAsChildOfStep(Transform collidingTransform, Transform stepTransform)
	{
		if (playerTransformReference == null)
		{
			playerTransformReference = collidingTransform;
		}

		stepParent = stepTransform;
		playerTransformReference.SetParent(stepTransform);
	}

	public void RemovePlayerAsChildOfStep(Transform stepTransform = null)
	{
		if (playerTransformReference == null)
			return;

		if (stepTransform != null && stepTransform != stepParent)
			return;

		playerTransformReference.SetParent(null);
		playerTransformReference = null;
	}

	private void ForegroundModeWasActivated()
	{
		RemovePlayerAsChildOfStep();

		foreach (var escalatorStep in escalatorStepsForEnablingAndDisablingColliders)
		{
			escalatorStep.gameObject.SetActive(false);
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
			escalatorStep.gameObject.SetActive(true);
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
		// Check for spawning and do the swapping including making sure the index 0 step has long collider enabled
		// This involves seeing if the first step would be on or go past position of second step,
		// and passing the difference (which could be 0,0,0).

		nextFramePositionOfCheckStep = stepsTransforms[GetIndexOfSwitchPositionStep()].position + (Vector3.left * escalatorSpeed * Time.deltaTime);
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
			for (int i = stepsTransforms.Count - 1; i >= GetIndexOfInnerTopHorizontalStep(); i--)
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
		for (int i = stepsTransforms.Count - 1; i > GetIndexOfInnerTopHorizontalStep(); i--)
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

		for (int i = stepsTransforms.Count - numberOfHorizontalSteps; i > GetIndexOfInnerBottomHorizontalStep(); i--)
		{
			MoveStepTowardsAimingPoint(i);
		}
	}

	private Vector3 amountToMoveStepSideways = Vector3.zero;
	private void MoveStepSideways(int index, float verticalPosition)
	{
		amountToMoveStepSideways.x = Vector3.left.x * escalatorSpeed * Time.deltaTime;
		stepsTransforms[index].position = stepsTransforms[index].position + amountToMoveStepSideways;

		if (stepsTransforms[index].position.y != verticalPosition)
		{
			stepsTransforms[index].position = new Vector3(stepsTransforms[index].position.x, verticalPosition, stepsTransforms[index].position.z);
		}
	}

	private void MoveStepTowardsAimingPoint(int index)
	{
		stepsTransforms[index].position = stepsTransforms[index].position + (diagonalVector * escalatorSpeed * Time.deltaTime);
	}

	private void DoRespawn()
	{
		// Do the list swapping stuff, ie move the first step to be the last step	
		firstStep = stepsTransforms[GetIndexOfFirstStep()];
		firstEscalatorStep = escalatorSteps[GetIndexOfFirstStep()];

		if (firstEscalatorStep.transform == stepParent)
			RemovePlayerAsChildOfStep();

		stepsTransforms.RemoveAt(GetIndexOfFirstStep());
		escalatorSteps.RemoveAt(GetIndexOfFirstStep());

		if (escalatorSpeed >= 0)
		{
			stepsTransforms.Insert(0, firstStep);
			stepsTransforms[0].position = stepsTransforms[1].position + horizontalStepsVector;

			stepsTransforms[GetIndexOfInnerBottomHorizontalStep()].position
				= stepsTransforms[GetIndexOfInnerBottomHorizontalStep() + 1].position - diagonalStepsVectorTop;

			escalatorSteps.Insert(0, firstEscalatorStep);
		}
		else
		{
			stepsTransforms.Add(firstStep);
			stepsTransforms[^1].position = stepsTransforms[^2].position - horizontalStepsVector;

			stepsTransforms[GetIndexOfInnerTopHorizontalStep()].position
				= stepsTransforms[GetIndexOfInnerTopHorizontalStep() - 1].position + diagonalStepsVectorTop;

			escalatorSteps.Add(firstEscalatorStep);
		}
	}

	private int GetIndexOfFirstStep()
	{
		if (escalatorSpeed >= 0)
			return stepsTransforms.Count - 1;
		else
			return 0;
	}

	private int GetIndexOfSwitchPositionStep()
	{
		if (escalatorSpeed >= 0)
			return stepsTransforms.Count - 1;
		else
			return 0;
	}

	private int GetIndexOfInnerTopHorizontalStep()
	{
		return stepsTransforms.Count - numberOfHorizontalSteps;
	}

	private int GetIndexOfInnerBottomHorizontalStep()
	{
		return numberOfHorizontalSteps - 1;
	}
}