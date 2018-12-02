using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement 
	: MonoBehaviour
{
	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	Rigidbody2D _rigidbody;
	Vector2 _targetPos;
	bool _reachedTarget = true;

	public float MovementSpeed = 10f;

	public delegate void PosDelegate(Vector2 pos);
	public event PosDelegate OnTargetReached;

	public void SetTarget(Vector2 pos)
	{
		_targetPos = pos;
		_reachedTarget = false;
	}
	
	void FixedUpdate ()
	{
		if (_reachedTarget)
			return;

		if (GameStateManager.Instance.State != GameStateManager.GameState.Moving)
			return;

		Vector2 dirToTarget = _targetPos - (Vector2)transform.position;
		float distToTarget = dirToTarget.magnitude;
		dirToTarget.Normalize();

		if (distToTarget <= float.Epsilon)
			return;

		float moveAmount = MovementSpeed * Time.deltaTime;
		bool bReachedTarget = distToTarget <= moveAmount;

		if (bReachedTarget)
		{
			moveAmount = distToTarget;
		}

		_rigidbody.rotation = Vector2.SignedAngle(Vector2.up, dirToTarget);
		_rigidbody.position += dirToTarget * moveAmount;

		if(bReachedTarget)
		{
			_reachedTarget = true;
			OnTargetReached(_targetPos);
		}
	}
}
