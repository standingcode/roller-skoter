using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : ValueObjectBase
{
	[SerializeField]
	private float healthValue;
	public float HealthValue { get { return healthValue; } protected set { healthValue = value; } }

	public override void UpdateValuesForPlayer()
	{

		Destroy(this.gameObject);
	}
}
