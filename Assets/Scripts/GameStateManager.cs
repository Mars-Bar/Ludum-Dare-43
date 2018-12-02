using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager 
	: MonoBehaviour
{
	private void Awake()
	{
		Instance = this;
	}

	public enum GameState
	{
		Moving,
		Placing,
		Paused,
	}

	public static GameStateManager Instance;

	public GameState State;

	public float GlobalSpawnRate = 1f;
}
