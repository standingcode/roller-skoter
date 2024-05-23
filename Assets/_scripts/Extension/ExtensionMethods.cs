using System.Linq;
using UnityEngine;

public static class ExtensionMethods
{

	private static int vector3ArrayLength = 0;
	private static Vector2[] vector2Array;
	public static Vector2[] Vector3ArrayToVector2Array(Vector3[] vector3Array)
	{
		vector3ArrayLength = vector3Array.Length;
		vector2Array = new Vector2[vector3ArrayLength];

		for (int i = 0; i < vector3ArrayLength; i++)
		{
			vector2Array[i] = vector3Array[i];
		}

		return vector2Array;
	}

	public static void IgnoreCollisionsBetweenTwoLayerMasks(LayerMask layerMask1, LayerMask layerMask2)
	{
		int[] layerIndexes1 = Enumerable.Range(0, 32).Where(index => layerMask1 == (layerMask1 | (1 << index))).ToArray();
		int[] layerIndexes2 = Enumerable.Range(0, 32).Where(index => layerMask2 == (layerMask2 | (1 << index))).ToArray();

		foreach (var layer in layerIndexes1)
		{
			foreach (var layer2 in layerIndexes2)
			{
				Physics2D.IgnoreLayerCollision(layer, layer2, true);
			}
		}
	}

	public static void AllowCollisionsBetweenTwoLayerMasks(LayerMask layerMask1, LayerMask layerMask2)
	{
		int[] layerIndexes1 = Enumerable.Range(0, 32).Where(index => layerMask1 == (layerMask1 | (1 << index))).ToArray();
		int[] layerIndexes2 = Enumerable.Range(0, 32).Where(index => layerMask2 == (layerMask2 | (1 << index))).ToArray();

		foreach (var layer in layerIndexes1)
		{
			foreach (var layer2 in layerIndexes2)
			{
				Physics2D.IgnoreLayerCollision(layer, layer2, false);
			}
		}
	}
}
