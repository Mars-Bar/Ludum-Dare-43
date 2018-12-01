using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObjectPlacement 
	: MonoBehaviour
{
	public GridObjectManager GridObjMgr;

	public Building ObjectPrefab;
	public Transform ObjectPreview;
	private Transform _objectPreviewGraphics;

	public SnapGrid Grid;
	public SnapGridGfx GridGraphics;

	GameStateManager.GameState _prevGameState = GameStateManager.GameState.Moving;

	public bool IsPlacing
	{
		get
		{
			return GameStateManager.Instance.State == GameStateManager.GameState.Placing;
		}
	}

	public void StartPlacing(Building prefab)
	{
		_prevGameState = GameStateManager.Instance.State;
		GameStateManager.Instance.State = GameStateManager.GameState.Placing;

		// show the grid
		GridGraphics.SetVisible(true);

		// assign the prefab
		ObjectPrefab = prefab;

		// create and attach the preview
		_objectPreviewGraphics = ObjectPrefab.CreatePreview();
		Vector2 gfxOffset = _objectPreviewGraphics.localPosition;
		_objectPreviewGraphics.parent = ObjectPreview;
		_objectPreviewGraphics.localPosition = gfxOffset;
	}

	public void StopPlacing()
	{
		GameStateManager.Instance.State = _prevGameState;

		// unassign the prefab
		//ObjectPrefab = null;

		// hide the grid
		GridGraphics.SetVisible(false);

		// hide the preview
		ObjectPreview.gameObject.SetActive(false);
		
		// destroy the graphics
		Destroy(_objectPreviewGraphics.gameObject);
	}

	private void Update()
	{
		if (!IsPlacing)
		{
			if (Input.GetButtonUp("TogglePlacing"))
				StartPlacing(ObjectPrefab);
			return;
		}

		// Update preview
		Vector2 position = HelperFuncs.GetMousePositionWorld();

		Vector2Int gridCoord = Grid.GetCoordFromCenter(position);
		position = Grid.GetCellCorner(gridCoord);

		bool isOccupied = GridObjMgr.IsOccupied(gridCoord, ObjectPrefab.OccupiedTiles);

		ObjectPreview.gameObject.SetActive(!isOccupied);
		ObjectPreview.transform.position = position;

		if(Input.GetButton("Place"))
		{
			if(!isOccupied)
			{
				Building building = Instantiate(ObjectPrefab, position, Quaternion.identity, GridObjMgr.transform);

				GridObjMgr.AddObject(building, gridCoord);
			}
		}

		if(Input.GetButtonUp("Cancel")
			|| Input.GetButtonUp("TogglePlacing"))
		{
			StopPlacing();
		}
	}
}
