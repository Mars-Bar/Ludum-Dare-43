﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager 
	: MonoBehaviour
{
	private void Awake()
	{
		Instance = this;
		_gridMgr = FindObjectOfType<GridObjectManager>();
	}

	private void Start()
	{
		ConstructFromGrid();
		UpdateCosts();
		Initialized = true;

		_gridMgr.OnObjectAdded += OnGridObjectModified;
		_gridMgr.OnObjectHealthChanged += OnGridObjectModified;
	}
	
	public bool Initialized { get; private set; }

	public static PathManager Instance { get; private set; }
	
	public bool NeedToUpdateCosts = true;
	private GridObjectManager _gridMgr;
	PathHeuristic _heuristic = new ManhattanHeuristic();
	private PathNode[,] _nodes;

	public HashSet<PathingAgent> PathingAgents = new HashSet<PathingAgent>();
	
	private void OnGridObjectModified(GridObject obj)
	{
		NeedToUpdateCosts = true;
	}

	public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end, float dps, float moveSpeed)
	{
		if (!Initialized)
			return null;

		PathNode startNode = _nodes[start.x, start.y];
		PathNode endNode = _nodes[end.x, end.y];

		List<PathNode> nodeList = FindPath(startNode, endNode, dps, moveSpeed);

		List<Vector2Int> result = new List<Vector2Int>();
		for(int i = 0; i < nodeList.Count; ++i)
		{
			if (nodeList[i] == startNode)
				continue;

			result.Add(nodeList[i].Position);
		}

		return result;
	}

	public void ConstructFromGrid()
	{
		Vector2Int gridSize = _gridMgr.PlacementGrid.CellCount;
		
		// fill array
		int w = gridSize.x, h = gridSize.y;
		_nodes = new PathNode[w, h];
		for (int y = 0; y < h; ++y)
		{
			for (int x = 0; x < w; ++x)
			{
				PathNode newNode = new PathNode();
				newNode.Position = new Vector2Int(x, y);
				newNode.CostToEnter = 0;
				_nodes[x,y] = newNode;
			}
		}

		Vector2Int[] neighbourOffsets = 
		{
			new Vector2Int(-1, 0), // left
			new Vector2Int( 1, 0), // right
			new Vector2Int( 0,-1), // bottom
			new Vector2Int( 0, 1), // top
			new Vector2Int(-1,-1), // BL
			new Vector2Int( 1,-1), // BR
			new Vector2Int(-1, 1), // TL
			new Vector2Int( 1, 1), // TR
		};

		// set up connections
		for (int y = 0; y < h; ++y)
		{
			for (int x = 0; x < w; ++x)
			{
				PathNode node = _nodes[x,y];

				// add each neighbour from the offsets
				foreach(Vector2Int n in neighbourOffsets)
				{
					int nX = x + n.x, nY = y + n.y;
					if (nX < 0 || nY < 0 || nX >= w || nY >= h)
						continue;

					PathNode.Neighbour neighbour = new PathNode.Neighbour();
					neighbour.Node = _nodes[nX, nY];
					// distance is 1.4 for diagonals, 1.0 for straights
					neighbour.Distance = n.sqrMagnitude > 1.1f ? 1.4f : 1.0f;
					node.Neighbours.Add(neighbour);
				}
			}
		}

		NeedToUpdateCosts = true;
	}

	public void UpdateCosts()
	{
		Vector2Int gridSize = _gridMgr.PlacementGrid.CellCount;

		int w = gridSize.x, h = gridSize.y;
		for (int y = 0; y < h; ++y)
		{
			for (int x = 0; x < w; ++x)
			{
				GridObject objOnNode = _gridMgr.GetObjectFromCoord(x, y);
				if (objOnNode != null)
				{
					_nodes[x, y].BlockageHealth = objOnNode.Health;
				}
			}
		}

		NeedToUpdateCosts = false;
	}

	class PathingInfo
	{
		public PathingInfo(PathNode n)
		{
			node = n;
			H = float.MaxValue;
			G = float.MaxValue;
		}
		public PathingInfo parent;
		public PathNode node;
		public float H;
		public float G;
	}

	static PathingInfo FindInSet(List<PathingInfo> set, PathNode node)
	{
		foreach(var pi in set)
		{
			if (pi.node == node)
				return pi;
		}
		return null;
	}

	private List<PathNode> FindPath(PathNode start, PathNode end, float blockageDPS, float moveSpeed)
	{
		List<PathNode> result = new List<PathNode>();
		if (start == end)
			return result;

		PathingInfo startInfo = new PathingInfo(start);
		startInfo.G = 0;
		startInfo.H = _heuristic.Calculate(start, end);

		List<PathingInfo> closedSet = new List<PathingInfo>();
		List<PathingInfo> openSet = new List<PathingInfo>() { startInfo };

		PathingInfo endInfo = null;

		while (endInfo == null && openSet.Count > 0)
		{
			// find tile with lowest score
			float minF = float.MaxValue;
			int minIndex = -1;
			for (int i = 0; i < openSet.Count; ++i)
			{
				float f = openSet[i].G + openSet[i].H;
				if (f < minF)
				{
					minF = f;
					minIndex = i;
				}
			}

			PathingInfo nodeInfo = openSet[minIndex];

			// move to closed set
			closedSet.Add(nodeInfo);
			openSet.RemoveAt(minIndex);

			var neighbours = nodeInfo.node.Neighbours;
			for (int i = 0; i < neighbours.Count; ++i)
			{
				var node = neighbours[i].Node;

				// Check if in closed set
				if (FindInSet(closedSet, node) != null)
					continue;

				float costToEnter = node.CostToEnter;
				if (node.BlockageHealth > 0)
				{
					costToEnter += node.BlockageHealth /= blockageDPS;
				}
				if(neighbours[i].Distance > 0f)
				{
					costToEnter += neighbours[i].Distance / moveSpeed;
				}
				float newG = nodeInfo.G + costToEnter;

				// Check if in open set
				PathingInfo pi = FindInSet(openSet, node);
				if (pi == null)
				{
					pi = new PathingInfo(node);

					// Compute score
					pi.G = newG;
					pi.H = _heuristic.Calculate(node, end);
					// set parent
					pi.parent = nodeInfo;

					// add to open set
					openSet.Add(pi);

					if (node == end)
					{
						// we have reached the end node
						endInfo = pi;
					}
				}
				else
				{
					// get F score
					float prevF = pi.G + pi.H;
					float newF = newG + pi.H;

					if (prevF < newF)
					{
						// cost along current path is better - use that
						pi.G = newG;
						pi.parent = nodeInfo;

						if (node == end)
						{
							// we have reached the end node
							endInfo = pi;
						}
					}
				}
			}
		}

		// construct the result path
		while (endInfo != null)
		{
			result.Insert(0, endInfo.node);
			endInfo = endInfo.parent;
		}
		//Debug.Log("Path Length" + result.Count.ToString());
		return result;
	}

	private void LateUpdate()
	{
		if (NeedToUpdateCosts)
		{
			UpdateCosts();
			foreach(PathingAgent agent in PathingAgents)
			{
				agent.UpdatePath();
			}
		}
	}
}
