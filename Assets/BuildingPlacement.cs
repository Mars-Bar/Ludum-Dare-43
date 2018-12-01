using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacement 
	: MonoBehaviour
{
	private void Awake()
	{
		_isPlacing = false;
		StartPlacing(BuildingPrefab);
	}

	public GridObjectManager GridObjMgr;

	public Building BuildingPrefab;
	private Transform _buildingPreview;

	public SnapGrid Grid;
	public SnapGridGfx GridGraphics;

	private bool _isPlacing = false;

	public void StartPlacing(Building prefab)
	{
		_isPlacing = true;
		GridGraphics.SetVisible(_isPlacing);

		BuildingPrefab = prefab;
		_buildingPreview = BuildingPrefab.CreatePreview();
	}

	public void StopPlacing()
	{
		_isPlacing = false;
		GridGraphics.SetVisible(_isPlacing);

		_buildingPreview = null;
	}

	private void Update()
	{
		if (!_isPlacing)
			return;

		// Update preview
		Vector2 position = HelperFuncs.GetMousePositionWorld();
		position = Grid.SnapToCellMid(position);
		_buildingPreview.transform.position = position;

		if(Input.GetButton("Place"))
		{
			GridObjMgr.CreateObject(BuildingPrefab);
		}
	}
}
