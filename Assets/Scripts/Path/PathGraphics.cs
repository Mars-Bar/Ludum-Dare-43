using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathGraphics 
	: MonoBehaviour
{
	private void Awake()
	{
		_lr = GetComponent<LineRenderer>();
	}

	public PathingAgent PathAgent;

	private LineRenderer _lr;

	private void Update()
	{
		Vector2[] pos2D = PathAgent.CurrentPath.ToArray();
		Vector3[] pos3D = new Vector3[pos2D.Length];
		for (int i = 0; i < pos2D.Length; ++i)
		{
			pos3D[i] = pos2D[i];
		}
		_lr.positionCount = pos3D.Length;
		_lr.SetPositions(pos3D);
	}
}
