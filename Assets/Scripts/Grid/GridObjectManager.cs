using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridObjectManager 
	: MonoBehaviour
{
	private void Awake()
	{
		foreach (var obj in FindObjectsOfType<GridObject>())
		{
			if (obj.AutoAddToGrid)
			{
				_gridObjects.Add(new GridObjectData(obj, PlacementGrid.GetCoordFromCorner(obj.transform.position)));
			}
		}
	}
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

	public delegate void ObjectEvent(GridObject obj);
	public event ObjectEvent OnObjectAdded;
	public event ObjectEvent OnObjectHealthChanged;

	public void ObjectHealthChanged(GridObject obj)
	{
		OnObjectHealthChanged(obj);
	}

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
		return GetObjectFromCoord(newCoord.x, newCoord.y);
	}

	public GridObject GetObjectFromCoord(int x, int y)
	{
		foreach (GridObjectData data in _gridObjects)
		{
			foreach (Vector2Int relCoord in data.Object.OccupiedTiles)
			{
				Vector2Int coord = data.Coord + relCoord;
				if (coord.x == x && coord.y == y)
					return data.Object;
			}
		}
		return null;
	}

	public Vector2Int[] GetCoordsFromObject(GridObject obj)
	{
		Vector2Int[] coords = null;
		foreach (GridObjectData data in _gridObjects)
		{
			if (data.Object == obj)
			{
				coords = new Vector2Int[obj.OccupiedTiles.Count];
				for (int i = 0; i < obj.OccupiedTiles.Count; ++i)
				{
					coords[i] = data.Coord + obj.OccupiedTiles[i];
				}
				break;
			}
		}
		return coords;
	}

	public Vector2[] GetPositionsFromObject(GridObject obj)
	{
		Vector2Int[] coords = GetCoordsFromObject(obj);
		if (coords == null)
			return null;

		Vector2[] positions = new Vector2[coords.Length];

		for(int i = 0; i < coords.Length; ++i)
		{
			positions[i] = PlacementGrid.GetCellCentre(coords[i]);
		}

		return positions;
	}

	public void AddObject(GridObject obj, Vector2Int coord)
	{
		Debug.Assert(GetObjectFromCoord(coord) == null);

		_gridObjects.Add(new GridObjectData(obj, coord));

		if(OnObjectAdded != null)
		{
			OnObjectAdded(obj);
		}
	}
}
