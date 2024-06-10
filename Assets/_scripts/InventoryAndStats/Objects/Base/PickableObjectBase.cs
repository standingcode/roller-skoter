using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
	None, CollectableObject, ValueObject
}

public abstract class PickableObjectBase : MonoBehaviour
{
	[SerializeField]
	protected ObjectType objectType;
	public ObjectType ObjectType { get => objectType; set => objectType = value; }

	[SerializeField]
	protected SpriteRenderer spriteRenderer;

	private bool pickupInProgress = false;
	public bool PickupInProgress { get => pickupInProgress; set => pickupInProgress = value; }

	[SerializeField]
	protected Collider2D collectableObjectCollider;

	public virtual void OnEnable()
	{
		if (collectableObjectCollider == null)
			collectableObjectCollider = GetComponent<Collider2D>();
	}

	public string GetSortingLayer()
	{
		return spriteRenderer.sortingLayerName;
	}

	public void HideObject()
	{
		gameObject.SetActive(false);
		collectableObjectCollider.enabled = false;
	}
}
