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

	void SetTargetPosition(Vector2 pos)
	{
		_reachedTarget = false;
		if (OnTargetReached != null)
		{
			OnTargetReached(pos);
		}
	}
	
	void FixedUpdate ()
	{
		if (_reachedTarget)
			return;

		if (GameStateManager.Instance.State != GameStateManager.GameState.Moving)
			return;

		Vector2 vecToTarget = _targetPos - (Vector2)transform.position;
		float distToTarget = vecToTarget.magnitude;

		if (distToTarget <= float.Epsilon)
			return;

		float moveAmount = MovementSpeed * Time.deltaTime;
		bool bReachedTarget = distToTarget <= moveAmount;

		if (bReachedTarget)
		{
			moveAmount = distToTarget;
		}

		_rigidbody.position += vecToTarget.normalized * moveAmount;

		if(bReachedTarget)
		{
			OnTargetReached(_targetPos);
		}
	}
}
