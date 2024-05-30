using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : ValueObjectBase
{
	[SerializeField]
	private float coinValue;
	public float CoinValue { get { return coinValue; } protected set { coinValue = value; } }

	public override void UpdateValuesForPlayer()
	{

		Destroy(this.gameObject);
	}
}
