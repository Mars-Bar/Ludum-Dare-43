using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToGridObjectOnSpawn 
	: MonoBehaviour
{
	private void Awake()
	{
		Spawner spawner = GetComponent<Spawner>();
		if(spawner != null)
		{
			spawner.SpawnEvent += OnObjectSpawned;
		}
	}

	public GridObject Target;

	public void OnObjectSpawned(GameObject obj)
	{
		MoveToGridObject move = obj.GetComponent<MoveToGridObject>();
		if (move == null)
			return;

		move.Target = Target;
	}
}
