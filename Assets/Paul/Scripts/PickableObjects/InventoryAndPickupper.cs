using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
	None, CollectableObject, ValueObject
}

[System.Serializable]
public class InventorySlot
{
	[SerializeField]
	int slotCounter = 5;

	[SerializeField]
	public List<CollectableObjectBase> collectableObjects = new List<CollectableObjectBase>();
	public List<CollectableObjectBase> CollectableObjects
	{
		get { return collectableObjects; }
		protected set { collectableObjects = value; }
	}

	public bool AddItem(CollectableObjectBase collectableObjectBase)
	{
		if (slotCounter - collectableObjectBase.Slotweight >= 0)
		{
			collectableObjects.Add(collectableObjectBase);
			slotCounter -= collectableObjectBase.Slotweight;

			return true;
		}
		else
		{
			return false;
		}
	}

	public void RemoveItem(CollectableObjectBase collectableObjectBase)
	{
		collectableObjects.Remove(collectableObjectBase);
		slotCounter += collectableObjectBase.Slotweight;
	}
}

public class InventoryAndPickupper : MonoBehaviour
{
	//[SerializeField]
	//private PlayerCharacter playerCharacter;

	[SerializeField]
	int amountOfInventorySlots = 10;

	private bool hasChestKey;
	public bool HasChestKey { get { return hasChestKey; } protected set { hasChestKey = value; } }

	private bool hasDoorKey;
	public bool HasDoorKey { get { return hasDoorKey; } protected set { hasDoorKey = value; } }

	//[SerializeField]
	//public List<PickableObjectBase> pickableObjects = new List<PickableObjectBase>();
	//public List<PickableObjectBase> PickableObjects

	[SerializeField]
	protected List<InventorySlot> inventorySlots = new List<InventorySlot>();
	public List<InventorySlot> InventorySlots
	{
		get { return inventorySlots; }
		protected set { inventorySlots = value; }
	}

	protected void OnEnable()
	{
		for (int i = 0; i < amountOfInventorySlots; i++)
		{
			InventorySlots.Add(new InventorySlot());
		}
	}

	protected virtual void OnTriggerEnter(Collider collider)
	{
		//Debug.LogError("Collided with something");
		CheckIfPickableObject(collider);
	}

	protected virtual void CheckIfPickableObject(Collider collider)
	{
		//Debug.Log($"A collision: {collider.name}");

		Rigidbody rigidbody = collider.attachedRigidbody;
		PickableObjectBase pickableObject = collider.GetComponentInParent<PickableObjectBase>();

		if (pickableObject != null)
		{
			//Debug.Log($"Collided with: {pickableObject.ObjectName}");

			//Item that is collected
			if (pickableObject.ObjectType == ObjectType.CollectableObject)
			{
				AddToInventory((CollectableObjectBase)pickableObject);
			}
			//Item which changes player stats such as money or health
			else if (pickableObject.ObjectType == ObjectType.ValueObject)
			{
				ValueObjectBase valueObjectBase = (ValueObjectBase)pickableObject;
			}
			else
			{
				Debug.LogError("I have no idea what to do with the object picked up");
			}

			pickableObject.HideObject();
		}
	}

	protected void AddToInventory(CollectableObjectBase collectableObjectBase)
	{
		for (int i = 0; i < InventorySlots.Count; i++)
		{
			if (InventorySlots[i].AddItem(collectableObjectBase) == true)
			{
				break;
			}
		}
	}
}

