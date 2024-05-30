using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObjectBase : PickableObjectBase
{
	public CollectableObjectScriptable CollectableObjectScriptable { get; private set; }

	private void OnEnable()
	{
		ObjectType = ObjectType.CollectableObject;
	}
}
