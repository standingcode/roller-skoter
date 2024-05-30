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
	private ObjectType objectType;
	public ObjectType ObjectType { get => objectType; set => objectType = value; }

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	public string GetSortingLayer()
	{
		return spriteRenderer.sortingLayerName;
	}

	public void HideObject()
	{
		gameObject.SetActive(false);
	}
}
