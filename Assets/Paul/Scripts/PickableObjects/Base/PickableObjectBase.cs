using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickableObjectBase : MonoBehaviour
{
	[SerializeField]
	private string objectName;
	public string ObjectName
	{
		get
		{
			return objectName;
		}
		protected set
		{
			objectName = value;
		}
	}

	private ObjectType objectType;
	public ObjectType ObjectType
	{
		get
		{
			return objectType;
		}
		protected set
		{
			objectType = value;
		}
	}

	public virtual void HideObject()
	{
		transform.gameObject.SetActive(false);
	}
}
