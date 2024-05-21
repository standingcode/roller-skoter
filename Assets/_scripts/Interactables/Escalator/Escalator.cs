using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Escalator : MonoBehaviour
{
	public bool Play = false;

	[SerializeField]
	private Transform slideSidewaysPoint, spawnPoint, killAimingPoint, aimingPoint, stepsRoot;

	[SerializeField]
	private BackgroundInteractable backgroundInteractable;

	[SerializeField]
	private EscalatorDirection escalatorDirection;

	[SerializeField]
	private float escalatorSpeed;

	private List<Transform> stepsTransforms = new();
	private List<EscalatorStep> escalatorSteps = new();

	private Transform stepParent;
	private Transform playerTransformReference;

	void Start()
	{
		for (int i = 0; i < stepsRoot.childCount; i++)
		{
			stepsTransforms.Add(stepsRoot.GetChild(i));
		}

		escalatorSteps = stepsRoot.gameObject.GetComponentsInChildren<EscalatorStep>(true).ToList();
		foreach (var escalatorStep in escalatorSteps)
		{
			escalatorStep.Escalator = this;
			escalatorStep.gameObject.SetActive(false);
		}

		backgroundInteractable.ForegroundModeActivated += RemovePlayerAsChildOfStep;
	}

	void FixedUpdate()
	{
		if (!Play)
			return;

		MoveAllStepsExceptFirstStepTowardsAimingPoint();
		MoveFirstStepTowardsKillPosition();
		CheckSecondStep();
	}

	public void SetPlayerAsChildOfStep(Transform collidingTransform, Transform stepTransform)
	{
		if (playerTransformReference == null)
		{
			playerTransformReference = collidingTransform;
		}

		stepParent = PlayerReferences.Instance.ConstantRayCasting.Hit.collider.transform;
		playerTransformReference.SetParent(PlayerReferences.Instance.ConstantRayCasting.Hit.collider.transform);
	}

	public void RemovePlayerAsChildOfStep()
	{
		if (playerTransformReference == null)
			return;

		playerTransformReference.SetParent(null);
		playerTransformReference = null;
	}

	private void CheckSecondStep()
	{
		if (
		(escalatorDirection == EscalatorDirection.UpLeft || escalatorDirection == EscalatorDirection.DownLeft) && (stepsTransforms[1].position.x <= slideSidewaysPoint.position.x)
		|| (escalatorDirection == EscalatorDirection.UpRight || escalatorDirection == EscalatorDirection.DownRight) && (stepsTransforms[1].position.x >= slideSidewaysPoint.position.x)
		)
		{
			SpawnAndReorder();
		}
	}

	private Transform firstStep;
	private EscalatorStep firstEscalatorStep;
	private void SpawnAndReorder()
	{
		// Reset second step position to the slide sideways position
		stepsTransforms[1].position = new Vector3(stepsTransforms[1].position.x, slideSidewaysPoint.position.y, stepsTransforms[1].position.z);


		// Do the list swapping stuff, ie move the first step to be the last step

		firstStep = stepsTransforms[0];
		firstEscalatorStep = escalatorSteps[0];

		if (firstEscalatorStep.transform == stepParent)
			RemovePlayerAsChildOfStep();

		stepsTransforms.RemoveAt(0);
		stepsTransforms.Add(firstStep);
		stepsTransforms[^1].position = spawnPoint.position;

		escalatorSteps.RemoveAt(0);
		escalatorSteps.Add(firstEscalatorStep);

	}

	private void MoveSecondStepTowardsAimingPoint()
	{
		stepsTransforms[1].position = Vector3.MoveTowards(stepsTransforms[1].position, aimingPoint.position, escalatorSpeed * Time.fixedDeltaTime);
	}

	private void MoveFirstStepTowardsKillPosition()
	{
		stepsTransforms[0].position = Vector3.MoveTowards(stepsTransforms[0].position, killAimingPoint.position, escalatorSpeed * Time.fixedDeltaTime);
	}

	private void MoveAllStepsExceptFirstStepTowardsAimingPoint()
	{
		for (int i = 1; i < stepsTransforms.Count; i++)
		{
			stepsTransforms[i].position = Vector3.MoveTowards(stepsTransforms[i].position, aimingPoint.position, escalatorSpeed * Time.fixedDeltaTime);
		}
	}
}

public enum EscalatorDirection
{
	UpLeft, UpRight, DownLeft, DownRight
}

