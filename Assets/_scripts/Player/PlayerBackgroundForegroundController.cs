using UnityEngine;
using UnityEngine.Rendering;

public class PlayerBackgroundForegroundController : MonoBehaviour
{
	private BackgroundInteractable currentBackgroundInteractable;
	public BackgroundInteractable CurrentBackgroundInteractable => currentBackgroundInteractable;

	[SerializeField]
	private SpriteRenderer characterSpriteRenderer;

	Color originalCharacterColor, currentCharacterColor;

	[SerializeField]
	private SortingGroup sortingGroup;

	private void Awake()
	{
		originalCharacterColor = characterSpriteRenderer.color;
	}

	public void ActivateBackgroundMode()
	{
		ChangeCharacterLayerToBackground();
		ChangeCharacterColorToBackground();
		CurrentBackgroundInteractable.ActivateBackgroundMode();
	}

	public void ActivateForegroundMode()
	{
		ChangeCharacterLayerToForeground();
		ChangeCharacterColorToForeground();
		CurrentBackgroundInteractable.ActivateForegroundMode();
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
		sortingGroup.sortingLayerID = SortingLayer.NameToID("Player");
	}

	public void ChangeCharacterLayerToBackground()
	{
		sortingGroup.sortingLayerID = SortingLayer.NameToID("PlayerBackgroundLayer");
	}

	public void AutoDeactivateBackgroundMode(AutoCancelBackgroundInteractable autoCancelBackgroundInteractable)
	{
		ChangeCharacterColorToForeground();
		ChangeCharacterLayerToForeground();
		autoCancelBackgroundInteractable.CancelBackgroundMode();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "BackgroundInteractable")
		{
			currentBackgroundInteractable = collision.gameObject.GetComponent<BackgroundInteractable>();
		}
		else if (collision.gameObject.tag == "AutoDeactivateBackgroundInteractable")
		{
			AutoDeactivateBackgroundMode(collision.gameObject.GetComponent<AutoCancelBackgroundInteractable>());
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
