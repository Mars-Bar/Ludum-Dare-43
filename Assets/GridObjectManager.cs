using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridObjectManager 
	: MonoBehaviour
{
	public SnapGrid PlacementGrid;

	[System.Serializable]
	struct GridObjectData
	{
		public GridObjectData(GridObject o, Vector2Int c)
		{
			Object = o;
			Coord = c;
		}
		public GridObject Object;
		public Vector2Int Coord;
	}
	private List<GridObjectData> _gridObjects = new List<GridObjectData>();

	public bool IsOccupied(Vector2Int startCoord, IEnumerable<Vector2Int> relativeCoords)
	{
		foreach(Vector2Int relCoord in relativeCoords)
		{
			Vector2Int coord = startCoord + relCoord;
			if (GetObjectFromCoord(coord) != null)
				return true;
		}
		return false;
	}

	public GridObject GetObjectFromCoord(Vector2Int newCoord)
	{
		foreach (GridObjectData data in _gridObjects)
		{
			foreach (Vector2Int relCoord in data.Object.OccupiedTiles)
			{
				Vector2Int coord = data.Coord + relCoord;
				if (coord == newCoord)
					return data.Object;
			}
		}
		return null;
	}

	public void AddObject(GridObject obj, Vector2Int coord)
	{
		Debug.Assert(GetObjectFromCoord(coord) == null);

		_gridObjects.Add(new GridObjectData(obj, coord));
	}
}
