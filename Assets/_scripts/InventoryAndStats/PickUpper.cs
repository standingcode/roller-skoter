using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpper : MonoBehaviour
{
	[SerializeField]
	private
		protected virtual void CheckIfPickableObject(Collider2D collider)
	{
		PickableObjectBase pickableObject = collider.GetComponent<PickableObjectBase>();

		if (
			pickableObject != null &&
			pickableObject.GetSortingLayer().Equals(PlayerReferences.Instance.PlayerBackgroundForegroundController.CharacterSpriteRenderer.sortingLayerName)
		)
		{
			if (pickableObject.IsPickedUp)
				return;

			pickableObject.HideObject();

			//Item that is collected
			if (pickableObject.ObjectType == ObjectType.CollectableObject)
			{
				StatsAndInventory.Instance.AddToInventory((CollectableObjectBase)pickableObject);
			}
			//Item which changes player stats such as money or health
			else if (pickableObject.ObjectType == ObjectType.ValueObject)
			{
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

