using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyType
{
	Door, Chest
}

public class Key : CollectableObjectBase
{
	[SerializeField]
	protected KeyType keyType;
	public KeyType KeyType { get { return keyType; } protected set { keyType = value; } }
}
