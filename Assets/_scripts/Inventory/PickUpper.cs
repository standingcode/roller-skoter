using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpper : MonoBehaviour
{
	protected virtual void CheckIfPickableObject(Collider2D collider)
	{
		//Debug.Log($"A collision: {collider.name}");

		PickableObjectBase pickableObject = collider.GetComponent<PickableObjectBase>();

		if (pickableObject != null)
		{
			//Debug.Log($"Collided with: {pickableObject.ObjectName}");

			//Item that is collected
			if (pickableObject.ObjectType == ObjectType.CollectableObject)
			{
				StatsAndInventory.Instance.AddToInventory((CollectableObjectBase)pickableObject);
			}
			//Item which changes player stats such as money or health
			else if (pickableObject.ObjectType == ObjectType.ValueObject)
			{
				ValueObjectBase valueObjectBase = (ValueObjectBase)pickableObject;

				// Now update the stats (Or try to)
				StatsAndInventory.Instance.UpdateStat(valueObjectBase);
			}
			else
			{
				Debug.LogError("I have no idea what to do with the object picked up");
			}

			pickableObject.HideObject();
		}
	}

	public void OnTriggerEnter2D(Collider2D collider)
	{
		CheckIfPickableObject(collider);
	}
}

