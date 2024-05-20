using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BackgroundInteractable : MonoBehaviour
{
	public static Action BackgroundModeActivated;
	public static Action ForegroundModeActivated;

	private bool backgroundModeActive = false;
	public bool BackgroundModeActive => backgroundModeActive;

	[SerializeField]
	private Color colorToChangeCharacterToInBackgroundMode;
	public Color ColorToChangeCharacterToInBackgroundMode => colorToChangeCharacterToInBackgroundMode;

	[SerializeField]
	private Transform[] transformsToActivateInBackgroundMode;

	private void Awake()
	{
		foreach (Transform transform in transformsToActivateInBackgroundMode)
		{
			transform.gameObject.SetActive(false);
		}
	}

	public void ActivateForegroundMode()
	{
		if (!backgroundModeActive)
			return;

		backgroundModeActive = false;

		foreach (Transform transform in transformsToActivateInBackgroundMode)
		{
			transform.gameObject.SetActive(false);
		}

		ForegroundModeActivated?.Invoke();
	}

	public void ActivateBackgroundMode()
	{
		if (backgroundModeActive)
			return;

		foreach (Transform transform in transformsToActivateInBackgroundMode)
		{
			transform.gameObject.SetActive(true);
		}

		backgroundModeActive = true;

		BackgroundModeActivated?.Invoke();
	}
}
