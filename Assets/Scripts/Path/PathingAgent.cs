using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingAgent 
	: MonoBehaviour
{
	protected virtual void Awake()
	{
		_movement = GetComponent<EnemyMovement>();
		_enemyAttack = GetComponent<EnemyAttack>();

		_movement.OnTargetReached += OnWaypointReached;

		_pathMgr = FindObjectOfType<PathManager>();
		_pathMgr.OnPathingChanged += OnPathingChanged;

		_nextTarget = _endTarget = transform.position;
	}

	PathManager _pathMgr;
	EnemyMovement _movement;
	EnemyAttack _enemyAttack;
	Vector2 _endTarget;
	Vector2 _nextTarget;

	public LinkedList<Vector2> CurrentPath = new LinkedList<Vector2>();

	private void Update()
	{
		SnapGrid snapGrid = SnapGrid.Instance;
		if(Vector2.Distance(transform.position, _nextTarget) > 1.5f)
		{
			// we have been moved away from where we were
			UpdatePath();
		}
	}

	public void SetEndTarget(Vector2 target)
	{
		_endTarget = target;
		UpdatePath();
	}

	void OnPathingChanged()
	{
		UpdatePath();
	}

	void UpdatePath()
	{
		SnapGrid snapGrid = SnapGrid.Instance;

		Vector2Int startCoord = snapGrid.GetCoordFromCentre(transform.position);
		Vector2Int endCoord = snapGrid.GetCoordFromCentre(_endTarget);

		CurrentPath = new LinkedList<Vector2>();

		List<Vector2Int> coordList = _pathMgr.FindPath(startCoord, endCoord, _enemyAttack.BlockageDPS);
		if (coordList != null && coordList.Count > 0)
		{
			for (int i = 0; i < coordList.Count; ++i)
			{
				Vector2 newWaypoint = snapGrid.GetCellCentre(coordList[i]);
				CurrentPath.AddLast(newWaypoint);
			}
		}

		OnWaypointReached(transform.position);
	}

	void OnWaypointReached(Vector2 waypoint)
	{
		if (CurrentPath.Count == 0)
			return;

		_nextTarget = CurrentPath.First.Value;
		CurrentPath.RemoveFirst();
		_movement.SetTarget(_nextTarget);
	}
}
