using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building 
	: GridObject
{
	public Transform Graphics;

	public Transform CreatePreview()
	{
		return Instantiate(Graphics, Graphics.localPosition, Graphics.localRotation);
	}
}
