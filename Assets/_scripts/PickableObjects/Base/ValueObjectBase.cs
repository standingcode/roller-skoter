using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ValueObjectBase : PickableObjectBase
{
	protected virtual void OnEnable()
	{
		ObjectType = ObjectType.ValueObject;
	}

	public abstract void UpdateValuesForPlayer();
}
