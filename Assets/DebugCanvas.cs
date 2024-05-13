using TMPro;
using UnityEngine;

public class DebugCanvas : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI somethingField;

	[SerializeField]
	private TextMeshProUGUI speedField;

	protected static DebugCanvas instance;
	public static DebugCanvas Instance { get => instance; }


	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(this);
		}
		else
		{
			instance = this;
		}
	}

	public void ShowSomething(string text)
	{
		somethingField.text = text;
	}

	public void ShowSpeed(float speed)
	{
		speedField.text = "Speed: " + speed.ToString("00.00");
	}
}
