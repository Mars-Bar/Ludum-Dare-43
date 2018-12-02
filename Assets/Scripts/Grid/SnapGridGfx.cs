using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapGridGfx 
	: MonoBehaviour
{
	public SnapGrid Grid;

	private SpriteRenderer _spriteRenderer;
	
	void Awake ()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		UpdateGraphics();
	}
	
	void UpdateGraphics()
	{
		Vector2 size = Grid.CellSize * Grid.CellCount;
		transform.localScale = Grid.CellSize;
		transform.localPosition = size / 2f;

		_spriteRenderer.size = Grid.CellCount;
	}

	public void SetVisible(bool bVisible)
	{
		_spriteRenderer.enabled = bVisible;
	}
}
