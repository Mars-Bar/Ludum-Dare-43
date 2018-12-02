using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming 
	: MonoBehaviour
{
	public float ProjectileSpeed = 10f;
	public float RotationSpeed = 10f;
	public Transform Target
	{
		get
		{
			return _target;
		}
		set
		{
			if (_target == value)
				return;

			_target = value;
			_prevTargetPosition = _target.position;
		}
	}
	private Transform _target;

	public bool OnTarget = false;

	public int numFramesToAverageOver = 100;
	private int _frameCounter = 0;
	private Vector2 _prevTargetPosition;
	private Vector2 _rollingAverageVelocity = Vector2.zero;

	// Predict movement based on the target's velocity
	private Vector2 GetAimPosition()
	{
		Vector2 targetVelocity = ((Vector2)Target.position - _prevTargetPosition) / Time.deltaTime;

		// estimate average rolling velocity
		if (_frameCounter > numFramesToAverageOver)
		{
			_rollingAverageVelocity -= _rollingAverageVelocity / numFramesToAverageOver;
			_rollingAverageVelocity += targetVelocity / numFramesToAverageOver;
		}
		else
		{
			_rollingAverageVelocity += (targetVelocity - _rollingAverageVelocity) / ++_frameCounter;
		}

		Vector2 toTarget = (Vector2)Target.position - (Vector2)transform.position;
		float distToTarget = toTarget.magnitude;
		toTarget.Normalize();

		float timeToTarget = distToTarget / ProjectileSpeed;
		Vector2 targetMovement = _rollingAverageVelocity * timeToTarget;

		return (Vector2)Target.position + targetMovement;
	}
	
	private void Update()
	{
		if (Target == null)
		{
			OnTarget = false;
			return;
		}

		Vector2 aimPos = GetAimPosition();

		Vector2 toTarget = aimPos - (Vector2)transform.position;
		toTarget.Normalize();
		float zAngle = Vector2.SignedAngle(Vector2.up, toTarget);
		
		transform.rotation = Quaternion.Euler(0f, 0f, zAngle);

		OnTarget = Vector2.SignedAngle(toTarget, transform.up) < 0.01f;
		
		_prevTargetPosition = Target.position;
	}
}
