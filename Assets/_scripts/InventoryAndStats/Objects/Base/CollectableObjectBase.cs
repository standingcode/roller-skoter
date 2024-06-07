using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObjectBase : PickableObjectBase
{
	[SerializeField]
	protected CollectableObjectScriptable collectableObjectScriptable;
	public CollectableObjectScriptable CollectableObjectScriptable { get => collectableObjectScriptable; set => collectableObjectScriptable = value; }

	private void OnEnable()
	{
		ObjectType = ObjectType.CollectableObject;
	}
}
