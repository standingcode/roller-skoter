using System.Collections.Generic;
using UnityEngine;

public class StatsAndInventory : MonoBehaviour
{
	public static StatsAndInventory Instance { get; private set; }

	[SerializeField]
	private int amountOfInventorySlots = 10;

	[SerializeField]
	protected List<CollectableObjectBase> inventoryObjects = new List<CollectableObjectBase>();
	public List<CollectableObjectBase> InventoryObjects { get { return inventoryObjects; } protected set { inventoryObjects = value; } }

	[SerializeField]
	protected int currentHealth;
	public int CurrentHealth { get => currentHealth; set => currentHealth = value; }

	[SerializeField]
	protected int currentSecurityAwareness;
	public int CurrentSecurityAwareness { get => currentSecurityAwareness; set => currentSecurityAwareness = value; }

	private void Start()
	{
		if (Instance != null)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

	public void AddToInventory(CollectableObjectBase collectableObjectBase)
	{
		Debug.Log("Updating inventory");

		if (inventoryObjects.Count < amountOfInventorySlots)
		{
			inventoryObjects.Add(collectableObjectBase);
			collectableObjectBase.gameObject.SetActive(false);
		}
		else
		{
			Debug.Log("Inventory is full");
		}
	}

	public void UpdateStat(ValueObjectBase valueObjectBase)
	{
		Debug.Log("Updating stats");
	}
}
