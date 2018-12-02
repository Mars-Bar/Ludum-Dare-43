using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
	: MonoBehaviour
{
	protected void Awake()
	{
		_gridObjectManager = FindObjectOfType<GridObjectManager>();

		HealthObj = GetComponentInChildren<HealthComponent>();
		HealthObj.OnHealthChanged += HealthChanged;

		// if this has no tiles, add the main tile
		if (OccupiedTiles.Count == 0)
			OccupiedTiles.Add(Vector2Int.zero);
	}
	public bool AutoAddToGrid = false;
	public List<Vector2Int> OccupiedTiles = new List<Vector2Int>() { Vector2Int.zero };

	public HealthComponent HealthObj { get; private set; }

	private GridObjectManager _gridObjectManager;

	public float Health
	{
		get
		{
			return HealthObj.Health;
		}
	}

	private void HealthChanged()
	{
		_gridObjectManager.ObjectHealthChanged(this);
	}
}
