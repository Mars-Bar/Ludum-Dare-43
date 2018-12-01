using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObjectManager 
	: MonoBehaviour
{
	public SnapGrid PlacementGrid;

	struct GridObjectData
	{
		public Vector2Int Coord;
		public GridObject Object;
	}
	private List<GridObjectData> _gridObjects;

	public GridObject GetObjectFromCoord(Vector2Int coord)
	{
		foreach (GridObjectData data in _gridObjects)
		{
			if (coord == data.Coord)
				return data.Object;
		}
		return null;
	}
}
