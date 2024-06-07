using TMPro;
using UnityEngine;

public class StatBar : MonoBehaviour
{
	[SerializeField]
	private RectTransform filledBar;

	[SerializeField]
	private TextMeshProUGUI valueText;

	[SerializeField]
	private float minValue = 0f;

	[SerializeField]
	private float maxValue = 100f;

	[SerializeField]
	private float currentValue = 100f;
	public float CurrentValue
	{
		get => currentValue;
		set
		{
			currentValue = Mathf.Clamp(value, minValue, maxValue);
			ChangeBarWidthAndTextValuesToMatchValue();
		}
	}

	public void Start()
	{
		ChangeBarWidthAndTextValuesToMatchValue();
	}

	[ContextMenu("ChangeBarWidthAndTextValuesToMatchValue")]
	public void ChangeBarWidthAndTextValuesToMatchValue()
	{
		filledBar.anchorMax = new Vector2(currentValue / maxValue, filledBar.anchorMax.y);
		valueText.text = currentValue.ToString("0");
	}
}
