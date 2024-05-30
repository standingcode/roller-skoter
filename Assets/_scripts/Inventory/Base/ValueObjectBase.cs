using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ValueObjectBase : PickableObjectBase
{
	public ValueObjectScriptable ValueObjectScriptable { get; private set; }
	public abstract void UpdateValuesForPlayer();

	private void OnEnable()
	{
		ObjectType = ObjectType.ValueObject;
	}
}
