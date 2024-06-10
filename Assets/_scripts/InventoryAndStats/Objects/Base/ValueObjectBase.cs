using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ValueObjectBase : PickableObjectBase
{
	[SerializeField]
	protected ValueObjectScriptable valueObjectScriptable;
	public ValueObjectScriptable ValueObjectScriptable { get => valueObjectScriptable; set => valueObjectScriptable = value; }

	public override void OnEnable()
	{
		ObjectType = ObjectType.ValueObject;

		base.OnEnable();
	}
}
