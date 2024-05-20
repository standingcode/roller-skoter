using UnityEngine;

public class AutoCancelBackgroundInteractable : MonoBehaviour
{
	[SerializeField]
	BackgroundInteractable backgroundInteractable;
	public void CancelBackgroundMode()
	{
		backgroundInteractable.ActivateForegroundMode();
	}
}
