using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapGridGfx 
	: MonoBehaviour
{
	public SnapGrid Grid;

	private Material _gridMaterial;
	
	void Awake ()
	{
		MeshRenderer mr = GetComponent<MeshRenderer>();
		_gridMaterial = mr.material;
		UpdateGraphics();
	}
	
	void UpdateGraphics()
	{
		Vector2 size = Grid.CellSize * Grid.CellCount;
		transform.localScale = size;
		transform.localPosition = size / 2f;

		_gridMaterial.mainTextureScale = Grid.CellCount;
	}

	public void SetVisible(bool bVisible)
	{
		GetComponent<MeshRenderer>().enabled = bVisible;
	}
}
