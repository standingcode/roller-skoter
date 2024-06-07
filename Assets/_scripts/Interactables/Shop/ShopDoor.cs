using UnityEngine;

[ExecuteAlways]
public class ShopDoor : MonoBehaviour
{
	[SerializeField]
	private Animator animator;

	[ContextMenu("OpenDoors")]
	public void OpenDoors()
	{
		animator.SetBool("DoorIsOpen", true);
	}

	[ContextMenu("CloseDoors")]
	public void CloseDoors()
	{
		animator.SetBool("DoorIsOpen", false);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
			OpenDoors();
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
			CloseDoors();
	}
}
