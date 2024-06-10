using System.Collections.Generic;
using UnityEngine;

public class StatsAndInventory : MonoBehaviour
{
	public static StatsAndInventory Instance { get; private set; }

	[SerializeField]
	private int amountOfInventorySlots = 0;

	[SerializeField]
	protected List<CollectableObjectBase> inventoryObjects = new List<CollectableObjectBase>();
	public List<CollectableObjectBase> InventoryObjects { get { return inventoryObjects; } protected set { inventoryObjects = value; } }

	[SerializeField]
	protected StatBar healthBar;
	public StatBar HealthBar { get => healthBar; set => healthBar = value; }

	[SerializeField]
	protected StatBar securityAwarenessBar;
	public StatBar SecurityAwarenessBar { get => securityAwarenessBar; set => securityAwarenessBar = value; }

	//[SerializeField]
	//protected int currentHealth;
	//public int CurrentHealth { get => currentHealth; set => currentHealth = value; }

	//[SerializeField]
	//protected int currentSecurityAwareness;
	//public int CurrentSecurityAwareness { get => currentSecurityAwareness; set => currentSecurityAwareness = value; }

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
			collectableObjectBase.HideObject();
		}
		else
		{
			Debug.Log("Inventory is full");
		}
	}

	public void UpdateStat(ValueObjectBase valueObjectBase)
	{
		Debug.Log("Updating stats");

		if (valueObjectBase is Health)
		{
			Health healthValueObject = valueObjectBase as Health;
			healthBar.CurrentValue += healthValueObject.ValueObjectScriptable.Value;
		}
		else if (valueObjectBase is Coin)
		{
			Coin coin = valueObjectBase as Coin;
		}
		else
		{
			Debug.Log("ValueObjectBase is not of type HealthValueObject or SecurityAwarenessValueObject");
		}
	}

	public void ClearInventorySlotImages()
	{

	}
}
