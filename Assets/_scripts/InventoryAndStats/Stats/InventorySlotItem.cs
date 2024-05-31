using Unity.Loading;
using UnityEngine;

public class InventorySlotItem : MonoBehaviour
{
	[SerializeField]
	private RectTransform rectTransform;

	[SerializeField]
	private int slotSize = 1;

	[ContextMenu("Set Slot Size")]
	public void SetSlotSize()
	{
		Rect rect = rectTransform.rect;
		rect.width *= slotSize;

		rectTransform.sizeDelta = new Vector2(rect.width, rect.height);
	}
}
