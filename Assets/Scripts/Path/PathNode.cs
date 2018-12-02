using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
	public struct Neighbour
	{
		public PathNode Node;
		public float DistanceFactor;
	}
	public float CostToEnter = 0;
	public float BlockageHealth = 0;
	public List<Neighbour> Neighbours = new List<Neighbour>();
	public Vector2Int Position { get; set; }
}
