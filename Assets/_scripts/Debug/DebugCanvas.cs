using TMPro;
using UnityEngine;

public class DebugCanvas : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI somethingField;

	[SerializeField]
	private TextMeshProUGUI speedField;

	[SerializeField]
	private TextMeshProUGUI heightField;

	[SerializeField]
	private Rigidbody2D mainRigidbody;

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

	public void ShowHeight(float height)
	{
		heightField.text = "Height: " + height.ToString("00.00");
	}

	private void Update()
	{
		if (mainRigidbody != null)
			ShowSpeed(mainRigidbody.velocityX);

		if (heightField != null)
			ShowHeight(PlayerReferences.Instance.PlayerControl.PlayerHeight);
	}
}
