using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBackgroundForegroundController : MonoBehaviour
{
	private BackgroundInteractable currentBackgroundInteractable;
	public BackgroundInteractable CurrentBackgroundInteractable => currentBackgroundInteractable;

	[SerializeField]
	private SpriteRenderer characterSpriteRenderer;
	public SpriteRenderer CharacterSpriteRenderer => characterSpriteRenderer;

	// Layer mask contains layers which should ignore each other in background mode
	public static Action<LayerMask> BackgroundModeActivated;
	public static Action ForegroundModeActivated;

	Color originalCharacterColor, currentCharacterColor;

	private void Awake()
	{
		originalCharacterColor = characterSpriteRenderer.color;
	}

	public void ActivateBackgroundMode()
	{
		if (string.IsNullOrEmpty(CurrentBackgroundInteractable.AlternateBackgroundLayerName))
			ChangeCharacterLayerToBackground();
		else
			ChangeCharacterLayerToBackground(CurrentBackgroundInteractable.AlternateBackgroundLayerName);

		ChangeCharacterColorToBackground();
		CurrentBackgroundInteractable.ActivateBackgroundMode();

		BackgroundModeActivated?.Invoke(CurrentBackgroundInteractable.LayersWhichShouldBeIgnoredWhenInBackgroundMode);
	}

	public void ActivateForegroundMode()
	{
		ChangeCharacterLayerToForeground();
		ChangeCharacterColorToForeground();
		currentBackgroundInteractable?.ActivateForegroundMode();
		//currentBackgroundInteractable = null;

		ForegroundModeActivated?.Invoke();
	}

	public void ChangeCharacterColorToForeground()
	{
		characterSpriteRenderer.color = originalCharacterColor;
	}

	public void ChangeCharacterColorToBackground()
	{
		currentCharacterColor = characterSpriteRenderer.color;
		currentCharacterColor = CurrentBackgroundInteractable.ColorToChangeCharacterToInBackgroundMode;
		characterSpriteRenderer.color = currentCharacterColor;
	}

	public void ChangeCharacterLayerToForeground()
	{
		characterSpriteRenderer.sortingLayerID = SortingLayer.NameToID("Player");
	}

	public void ChangeCharacterLayerToBackground(string backgroundLayerName = "PlayerBackgroundLayer")
	{
		characterSpriteRenderer.sortingLayerID = SortingLayer.NameToID(backgroundLayerName);
	}

	public void AutoDeactivateBackgroundMode(AutoCancelBackgroundInteractable autoCancelBackgroundInteractable)
	{
		ActivateForegroundMode();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "BackgroundInteractable")
		{
			currentBackgroundInteractable = collision.gameObject.GetComponent<BackgroundInteractable>();
		}
		else if (collision.gameObject.tag == "AutoDeactivateBackgroundInteractable")
		{
			ActivateForegroundMode();
			collision.gameObject.GetComponent<AutoCancelBackgroundInteractable>().CancelBackgroundMode();
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "BackgroundInteractable")
		{
			currentBackgroundInteractable = null;
		}
	}
}
