using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner 
	: MonoBehaviour
{
	private void Start()
	{
		GenerateNewInterval();	
	}

	public Transform SpawnParent;
	public GameObject Prefab;
	public float MinSpawnInterval = 1f;
	public float MaxSpawnInterval = 5f;

	public delegate void SpawnDelegate(GameObject newObj);
	public event SpawnDelegate SpawnEvent;

	private float _spawnInterval = 0f;
	private float _spawnTimer = 0f;

	private void Update()
	{
		if (GameStateManager.Instance.State != GameStateManager.GameState.Moving)
			return;

		_spawnTimer += Time.deltaTime;
		if(_spawnTimer > _spawnInterval)
		{
			Spawn();
			GenerateNewInterval();
		}
	}

	private void Spawn()
	{
		GameObject newObj = Instantiate(Prefab, transform.position, transform.rotation, SpawnParent);
		if(SpawnEvent != null)
		{
			SpawnEvent(newObj);
		}
	}

	private void GenerateNewInterval()
	{
		_spawnInterval = Random.Range(MinSpawnInterval, MaxSpawnInterval) / GameStateManager.Instance.GlobalSpawnRate;

		_spawnTimer = 0f;
	}
}
