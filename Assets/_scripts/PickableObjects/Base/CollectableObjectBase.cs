using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectableObjectBase : PickableObjectBase
{
	[SerializeField]
	private int slotweight;
	public int Slotweight
	{
		get
		{
			return slotweight;
		}
		protected set
		{
			slotweight = value;
		}
	}

	protected virtual void OnEnable()
	{
		ObjectType = ObjectType.CollectableObject;
	}
}
