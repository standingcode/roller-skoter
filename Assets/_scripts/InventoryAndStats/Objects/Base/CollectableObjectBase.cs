using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObjectBase : PickableObjectBase
{
	[SerializeField]
	protected CollectableObjectScriptable collectableObjectScriptable;
	public CollectableObjectScriptable CollectableObjectScriptable { get => collectableObjectScriptable; set => collectableObjectScriptable = value; }

	[SerializeField]
	private Stack<Collider2D> collidersWhichEnteredObject = new();

	public override void OnEnable()
	{
		ObjectType = ObjectType.CollectableObject;

		base.OnEnable();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		collidersWhichEnteredObject.Push(collision);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collidersWhichEnteredObject.Count > 0)
			collidersWhichEnteredObject.Pop();

		if (collidersWhichEnteredObject.Count == 0)
		{
			PickupInProgress = false;
		}
	}
}
