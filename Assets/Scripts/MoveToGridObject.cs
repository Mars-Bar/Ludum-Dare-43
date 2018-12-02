using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToGridObject
	: MonoBehaviour
{
	private void Awake()
	{
		_agent = GetComponent<PathingAgent>();
		if (GridMgr == null)
			GridMgr = FindObjectOfType<GridObjectManager>();
	}

	private void Start()
	{
		Vector2 myPosition = transform.position;
		Vector2 targetPos = myPosition;
		float minSqDist = float.MaxValue;

		// Find the closest square that the target is on
		Vector2[] objectPositions = GridMgr.GetPositionsFromObject(Target);
		if (objectPositions != null)
		{
			for (int i = 0; i < objectPositions.Length; ++i)
			{
				float sqDist = (objectPositions[i] - myPosition).sqrMagnitude;
				if (sqDist < minSqDist)
				{
					minSqDist = sqDist;
					targetPos = objectPositions[i];
				}
			}
		}

		_agent.SetEndTarget(targetPos);
	}

	public GridObject Target;
	public GridObjectManager GridMgr;

	private PathingAgent _agent;
}
