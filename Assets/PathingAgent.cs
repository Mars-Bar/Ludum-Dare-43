using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingAgent 
	: MonoBehaviour
{
	Vector2 _endTarget;

	void SetEndTarget(Vector2 target)
	{
		_endTarget = target;
	}
}
