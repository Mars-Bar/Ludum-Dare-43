using UnityEngine;
using System.Collections;

public static class HelperFuncs
{
	public static Vector3 ScreenToWorldPos(Vector3 screenPos, Camera camera)
	{
		return camera.ScreenToWorldPoint(screenPos);
	}

	public static Vector3 ScreenToWorldPos(Vector3 screenPos)
	{
		return ScreenToWorldPos(screenPos, Camera.main);
	}

	public static Vector3 GetMousePositionWorld()
	{
		return ScreenToWorldPos(Input.mousePosition, Camera.main);
	}
}
