using UnityEngine;
using UnityEngine.UI;

public class InventorySlotItem : MonoBehaviour
{
	[SerializeField]
	private RectTransform rectTransform;

	[SerializeField]
	private Image image;

	protected CollectableObjectBase collectableObjectBase;
	public CollectableObjectBase CollectableObjectBase { get => collectableObjectBase; set => collectableObjectBase = value; }

	//[SerializeField]
	//private int slotSize = 1;

	//[ContextMenu("Set Slot Size")]
	//public void SetSlotSize()
	//{
	//	Rect rect = rectTransform.rect;
	//	rect.width *= slotSize;

	//	rectTransform.sizeDelta = new Vector2(rect.width, rect.height);
	//}

	public void SetImage(Sprite sprite)
	{
		image.sprite = sprite;
		Color color = image.color;
		color.a = 255;
		image.color = color;
	}

	[ContextMenu("Clear Image")]
	public void ClearImage()
	{
		Color color = image.color;
		color.a = 0;
		image.color = color;
	}
}
