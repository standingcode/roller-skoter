using UnityEngine;

public class PlayerReferences : MonoBehaviour
{
	[SerializeField]
	private ConstantRayCasting constantRayCasting;
	public ConstantRayCasting ConstantRayCasting => constantRayCasting;

	[SerializeField]
	private PlayerControl playerControl;
	public PlayerControl PlayerControl => playerControl;

	[SerializeField]
	private PlayerManager playerManager;
	public PlayerManager PlayerManager => playerManager;

	[SerializeField]
	private PlayerBackgroundForegroundController playerBackgroundForegroundController;
	public PlayerBackgroundForegroundController PlayerBackgroundForegroundController => playerBackgroundForegroundController;

	[SerializeField]
	private LayerMask playerLayers;
	public LayerMask PlayerLayers => playerLayers;


	protected static PlayerReferences instance;
	public static PlayerReferences Instance { get => instance; }

	void Awake()
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
