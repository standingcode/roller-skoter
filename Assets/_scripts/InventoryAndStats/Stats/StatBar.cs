using TMPro;
using UnityEngine;

[ExecuteAlways]
public class StatBar : MonoBehaviour
{
	[SerializeField]
	[Range(0, 100)]
	private int value;
	public int Value
	{
		get => value;
		set
		{
			this.value = value;
			ChangeBarWidthAndTextValuesToMatchValue();
		}
	}

	[SerializeField]
	private RectTransform filledBar;

	[SerializeField]
	private TextMeshProUGUI valueText;

	public void ChangeBarWidthAndTextValuesToMatchValue()
	{
		filledBar.anchorMax = new Vector2(value / 100f, filledBar.anchorMax.y);
		valueText.text = value.ToString();
	}

	private void Update()
	{
		ChangeBarWidthAndTextValuesToMatchValue();
	}
}
