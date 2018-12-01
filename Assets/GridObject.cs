using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject 
	: MonoBehaviour
{
	private void Awake()
	{
		// if this has no tiles, add the main tile
		if (OccupiedTiles.Count == 0)
			OccupiedTiles.Add(Vector2Int.zero);

		if (AutoAddToGrid)
		{
			SnapGrid grid = FindObjectOfType<SnapGrid>();
			GridObjectManager manager = FindObjectOfType<GridObjectManager>();
			manager.AddObject(this, grid.GetCoordFromCorner(transform.position));
		}
	}
	public bool AutoAddToGrid = false;
	public List<Vector2Int> OccupiedTiles = new List<Vector2Int>() { Vector2Int.zero };
}
