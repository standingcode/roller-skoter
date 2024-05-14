using UnityEngine;

public class PlayerReferences : MonoBehaviour
{
	[SerializeField]
	private ConstantRayCasting constantRayCasting;
	public ConstantRayCasting ConstantRayCasting => constantRayCasting;

	[SerializeField]
	private PlayerControl playerControl;
	public PlayerControl PlayerControl => playerControl;


	protected static PlayerReferences instance;
	public static PlayerReferences Instance { get => instance; }

	void Start()
	{
		if (Instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this);
		}
	}
}
