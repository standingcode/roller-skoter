using System;
using UnityEngine;

public class BackgroundInteractable : MonoBehaviour
{
	public Action BackgroundModeActivated;
	public Action ForegroundModeActivated;

	private bool backgroundModeActive = false;
	public bool BackgroundModeActive => backgroundModeActive;

	[SerializeField]
	private Color colorToChangeCharacterToInBackgroundMode;
	public Color ColorToChangeCharacterToInBackgroundMode => colorToChangeCharacterToInBackgroundMode;

	[SerializeField]
	private Transform[] transformsToActivateInBackgroundMode;

	[SerializeField]
	private Transform[] transformsToDeactivateInBackgroundMode;

	[SerializeField]
	private LayerMask layersWhichShouldBeIgnoredWhenInBackgroundMode;

	[SerializeField]
	private string alternateBackgroundLayerName = null;
	public string AlternateBackgroundLayerName => alternateBackgroundLayerName;

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

		ForegroundModeActivated?.Invoke();

		ExtensionMethods.AllowCollisionsBetweenTwoLayerMasks(PlayerReferences.Instance.PlayerLayers, layersWhichShouldBeIgnoredWhenInBackgroundMode);

		backgroundModeActive = false;

		foreach (Transform activateTransform in transformsToActivateInBackgroundMode)
		{
			activateTransform.gameObject.SetActive(false);
		}

		foreach (Transform deactivateTransform in transformsToDeactivateInBackgroundMode)
		{
			deactivateTransform.gameObject.SetActive(true);
		}
	}

	public void ActivateBackgroundMode()
	{
		if (backgroundModeActive)
			return;

		BackgroundModeActivated?.Invoke();

		ExtensionMethods.IgnoreCollisionsBetweenTwoLayerMasks(PlayerReferences.Instance.PlayerLayers, layersWhichShouldBeIgnoredWhenInBackgroundMode);

		foreach (Transform activateTransform in transformsToActivateInBackgroundMode)
		{
			activateTransform.gameObject.SetActive(true);
		}

		foreach (Transform deactivateTransform in transformsToDeactivateInBackgroundMode)
		{
			deactivateTransform.gameObject.SetActive(false);
		}

		backgroundModeActive = true;
	}
}
