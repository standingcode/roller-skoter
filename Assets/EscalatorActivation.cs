using UnityEngine;

public class EscalatorActivation : MonoBehaviour
{
	[SerializeField]
	Escalator escalator;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("Something");
	}
}
