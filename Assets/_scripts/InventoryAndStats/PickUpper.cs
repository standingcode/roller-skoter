using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpper : MonoBehaviour
{
	protected virtual void CheckIfPickableObject(Collider2D collider)
	{
		PickableObjectBase pickableObject = collider.GetComponent<PickableObjectBase>();

		if (
			pickableObject != null &&
			pickableObject.GetSortingLayer().Equals(PlayerReferences.Instance.PlayerBackgroundForegroundController.CharacterSpriteRenderer.sortingLayerName)
		)
		{
			if (pickableObject.PickupInProgress)
				return;

			pickableObject.PickupInProgress = true;

			//Item that is collected
			if (pickableObject.ObjectType == ObjectType.CollectableObject)
			{
				// Item gets hidden by the inventory if inventory is not full and item is picked
				StatsAndInventory.Instance.AddToInventory((CollectableObjectBase)pickableObject);
			}
			//Item which changes player stats such as money or health
			else if (pickableObject.ObjectType == ObjectType.ValueObject)
			{
				pickableObject.HideObject();
				ValueObjectBase valueObjectBase = (ValueObjectBase)pickableObject;
				StatsAndInventory.Instance.UpdateStat(valueObjectBase);
			}
			else
			{
				Debug.LogError("I have no idea what to do with the object picked up");
			}
		}
	}

	public void OnTriggerEnter2D(Collider2D collider)
	{
		CheckIfPickableObject(collider);
	}
}

