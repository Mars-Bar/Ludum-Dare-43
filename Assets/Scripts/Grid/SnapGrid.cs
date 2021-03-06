﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapGrid
	: MonoBehaviour
{
	private void Awake()
	{
		Instance = this;
	}

	public static SnapGrid Instance { get; private set; }

	public Vector2 CellSize = Vector2.one;
	public Vector2Int CellCount = new Vector2Int(10, 10);

	public Vector2 GetCellCentre(Vector2Int coord, Space space = Space.World)
	{
		Vector2 position = GetCellCorner(coord, space);

		position += CellSize / 2.0f;

		return position;
	}

	public Vector2 GetCellCorner(Vector2Int coord, Space space = Space.World)
	{
		Vector2 position = (space == Space.World) ? (Vector2)transform.position : Vector2.zero;
		if (coord.x > CellCount.x || coord.y > CellCount.y)
		{
			Debug.LogError(string.Format("Grid coordinates out of range:\nX={0}\nY={1}", coord.x, coord.y));
			return position;
		}

		position += new Vector2(
			coord.x * CellSize.x,
			coord.y * CellSize.y);

		return position;
	}

	public Vector2Int GetCoordFromCentre(Vector2 position, Space space = Space.World)
	{
		position -= CellSize / 2.0f;
		return GetCoordFromCorner(position, space);
	}

	public Vector2Int GetCoordFromCorner(Vector2 position, Space space = Space.World)
	{
		if(space == Space.World)
			position -= (Vector2)transform.position;

		Vector2Int coord = new Vector2Int(
			Mathf.RoundToInt(position.x / CellSize.x), 
			Mathf.RoundToInt(position.y / CellSize.y));

		coord.x = Mathf.Clamp(coord.x, 0, CellCount.x - 1);
		coord.y = Mathf.Clamp(coord.y, 0, CellCount.y - 1);

		return coord;
	}

	public Vector2 SnapToCellCorner(Vector2 position, Space space = Space.World)
	{
		Vector2Int coord = GetCoordFromCorner(position, space);

		return GetCellCorner(coord, space);
	}

	public Vector2 SnapToCellCentre(Vector2 position, Space space = Space.World)
	{
		Vector2Int coord = GetCoordFromCentre(position, space);

		return GetCellCentre(coord, space);
	}
}
