using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Hierarchy;
using UnityEngine;

public class Escalator : MonoBehaviour
{
	public bool Play = false;

	[SerializeField]
	private Transform stepsRoot;

	private Vector3 spawnPoint, switchPoint;
	private float topStepYMax;

	[SerializeField]
	private BackgroundInteractable backgroundInteractable;

	[SerializeField]
	private EscalatorDirection escalatorDirection;

	[SerializeField]
	private float escalatorSpeed;

	[SerializeField]
	private List<Transform> stepsTransforms = new();

	[SerializeField]
	private List<EscalatorStep> escalatorSteps = new();

	private Transform stepParent;
	private Transform playerTransformReference;

	[SerializeField]
	private List<GameObject> gameObjectsToActivateOnBackgroundMode;

	void Start()
	{
		// Get the transform root of each step
		for (int i = 0; i < stepsRoot.childCount; i++)
		{
			stepsTransforms.Add(stepsRoot.GetChild(i));
		}

		// Get all the escalator step scripts and add them to a list, whilst also passing them reference to this Escalator script
		escalatorSteps = stepsRoot.gameObject.GetComponentsInChildren<EscalatorStep>(true).ToList();
		foreach (var escalatorStep in escalatorSteps)
		{
			escalatorStep.Escalator = this;
			escalatorStep.gameObject.SetActive(false);
		}

		// Set the positions needed for movement
		spawnPoint = stepsTransforms[0].position;
		switchPoint = stepsTransforms[1].position;
		topStepYMax = stepsTransforms[^1].position.y;

		// Subscribe to the background interactable events
		backgroundInteractable.ForegroundModeActivated += ForegroundModeWasActivated;
		backgroundInteractable.BackgroundModeActivated += BackgroundModeWasActivated;

		foreach (var gameObject in gameObjectsToActivateOnBackgroundMode)
		{
			gameObject.SetActive(false);
		}
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

		foreach (var escalatorStep in escalatorSteps)
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
		foreach (var escalatorStep in escalatorSteps)
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
	private Vector3 aimingVector = new Vector3(-1, 1, 0);
	private Vector3 nextFramePositionOfLastStep;
	private void CheckForSpawningAndMoveSteps()
	{
		// Check for spawning and do the swapping including making sure the index 0 step has long collider enabled
		// This involves seeing if the first step would be on or go past position of second step, and passing the difference (which could be 0,0,0).
		nextFramePositionOfLastStep = stepsTransforms[0].position + (aimingVector * escalatorSpeed * Time.deltaTime);
		if (nextFramePositionOfLastStep.x <= switchPoint.x || nextFramePositionOfLastStep.y >= switchPoint.y)
		{
			DoRespawn(nextFramePositionOfLastStep - switchPoint);
		}

		// Now the last index step always moves sideways
		stepsTransforms[^1].position = stepsTransforms[^1].position + (Vector3.left * escalatorSpeed * Time.deltaTime);

		if (stepsTransforms[^1].position.y > topStepYMax)
		{
			stepsTransforms[^1].position = new Vector3(stepsTransforms[^1].position.x, topStepYMax, stepsTransforms[^1].position.z);
		}

		// The rest of the steps move towards the aiming point
		for (int i = 0; i < stepsTransforms.Count - 1; i++)
		{
			stepsTransforms[i].position = stepsTransforms[i].position + (aimingVector * escalatorSpeed * Time.deltaTime);

			if (stepsTransforms[i].position.y > topStepYMax)
			{
				stepsTransforms[i].position = new Vector3(stepsTransforms[i].position.x, topStepYMax, stepsTransforms[i].position.z);
			}
		}
	}

	private void DoRespawn(Vector3 positionPastSwitchPoint)
	{
		// Do the list swapping stuff, ie move the first step to be the last step

		firstStep = stepsTransforms[^1];
		firstEscalatorStep = escalatorSteps[^1];

		if (firstEscalatorStep.transform == stepParent)
			RemovePlayerAsChildOfStep();

		stepsTransforms.RemoveAt(stepsTransforms.Count - 1);
		stepsTransforms.Insert(0, firstStep);
		stepsTransforms[0].position = spawnPoint + positionPastSwitchPoint;

		escalatorSteps.RemoveAt(escalatorSteps.Count - 1);
		firstEscalatorStep.SetShortCollider();
		escalatorSteps.Insert(0, firstEscalatorStep);

		escalatorSteps[escalatorSteps.Count - 1].SetLongCollider();
	}
}

public enum EscalatorDirection
{
	UpLeft, UpRight, DownLeft, DownRight
}
