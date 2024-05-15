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
}
