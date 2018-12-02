using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathHeuristic
{
	public abstract float Calculate(PathNode start, PathNode end);
}

public class ManhattanHeuristic
	: PathHeuristic
{
	public override float Calculate(PathNode start, PathNode end)
	{
		return Mathf.Abs(start.Position.x - end.Position.x)
			+ Mathf.Abs(start.Position.y - end.Position.y);
	}
}

public class EuclideanHeuristic
	: PathHeuristic
{
	public override float Calculate(PathNode start, PathNode end)
	{
		return Vector2.Distance(start.Position, end.Position);
	}
}
