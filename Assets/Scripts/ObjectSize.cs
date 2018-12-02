using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSize 
	: MonoBehaviour
{
	public float Width = 1;
	public float Height = 1;
	public enum ObjAnchor
	{
		BottomLeft,
		Centre,
	}
	public ObjAnchor Anchor;
}
