using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget
	: MonoBehaviour
{
	private void Awake()
	{
		_agent = GetComponent<PathingAgent>();
	}

	private void Start()
	{
		_agent.SetEndTarget(Target.position);
	}
	public Transform Target;

	private PathingAgent _agent;
}
