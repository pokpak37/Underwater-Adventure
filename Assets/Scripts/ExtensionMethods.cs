using UnityEngine;
using System.Collections;

public static class ExtensionMethods
{
	static Vector3 position;
	public static void SetX(this Transform newTransform,float newX)
	{
			position = newTransform.position;
			position.x = newX;
			newTransform.position = position;
	}
	public static void SetY(this Transform newTransform,float newY)
	{
		position = newTransform.position;
		position.y = newY;
		newTransform.position = position;
	}
	public static void SetZ(this Transform newTransform,float newZ)
	{
		position = newTransform.position;
		position.z = newZ;
		newTransform.position = position;
	}

	public static void SetXYZ(this Transform newTransform,float newX,float newY,float newZ)
	{
		position = newTransform.position;
		position.x = newX;
		position.y = newY;
		position.z = newZ;
		newTransform.position = position;
	}

	public static void SetXZ(this Transform newTransform,float newX,float newZ)
	{
		position = newTransform.position;
		position.x = newX;
		position.z = newZ;
		newTransform.position = position;
	}

	public static void SetXY(this Transform newTransform,float newX,float newY)
	{
		position = newTransform.position;
		position.x = newX;
		position.y = newY;
		newTransform.position = position;
	}
	public static void SetYZ(this Transform newTransform,float newY,float newZ)
	{
		position = newTransform.position;
		position.y = newY;
		position.z = newZ;
		newTransform.position = position;
	}
}