using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIncreaser : ValueObjectBase
{
	[SerializeField]
	private float amountToIncreaseMaxHealthBy = 2.5f;
	public float AmountToIncreaseMaxHealthBy
	{
		get
		{
			return amountToIncreaseMaxHealthBy;
		}
		protected set
		{
			amountToIncreaseMaxHealthBy = value;
		}
	}

	public override void UpdateValuesForPlayer()
	{

		Destroy(this.gameObject);
	}
}
