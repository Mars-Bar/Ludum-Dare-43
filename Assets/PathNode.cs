using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
	public struct Neighbour
	{
		public PathNode Node;
		public float Cost;
	}
	public List<Neighbour> Neighbours;

}
