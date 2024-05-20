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
		sortingGroup.sortingLayerID = SortingLayer.NameToID("PlayerBackgroundLayer");

		currentCharacterColor = characterSpriteRenderer.color;
		currentCharacterColor = CurrentBackgroundInteractable.ColorToChangeCharacterToInBackgroundMode;
		characterSpriteRenderer.color = currentCharacterColor;

		CurrentBackgroundInteractable.ActivateBackgroundMode();
	}

	public void ActivateForegroundMode()
	{
		sortingGroup.sortingLayerID = SortingLayer.NameToID("Player");

		characterSpriteRenderer.color = originalCharacterColor;

		CurrentBackgroundInteractable.ActivateForegroundMode();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "BackgroundInteractable")
		{
			currentBackgroundInteractable = collision.gameObject.GetComponent<BackgroundInteractable>();
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
