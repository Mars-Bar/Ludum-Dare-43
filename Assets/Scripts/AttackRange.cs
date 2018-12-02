using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackRange 
	: MonoBehaviour
{
	protected virtual void Awake()
	{
		_collider = GetComponent<Collider2D>();
	}

	private Collider2D _collider;

	public class Target
	{
		public Collider2D Collider;
		public HealthComponent HealthObj;
		public Vector2 PrevPos;
		public float Dist;
	}

	public List<Target> TargetsInRange = new List<Target>();

	private void OnTriggerEnter2D(Collider2D other)
	{
		HealthCollider hc = other.GetComponent<HealthCollider>();
		if(hc != null)
		{
			float dist = CalculateDistToCollider(other);

			Target t = new Target();
			t.HealthObj = hc.HealthObj;
			t.Collider = other;
			t.Dist = dist;
			t.PrevPos = hc.HealthObj.transform.position;
			TargetsInRange.Add(t);
			TargetsInRange.OrderBy(x => x.Dist);
		}
	}

	private float CalculateDistToCollider(Collider2D other)
	{
		var dist = _collider.Distance(other);
		Debug.Assert(dist.isValid);
		return dist.distance;
	}

	private void Update()
	{
		if (TargetsInRange.Count == 0)
			return;

		// remove null distances
		for(int i = TargetsInRange.Count - 1; i >= 0; --i)
		{
			if(TargetsInRange[i].HealthObj == null)
			{
				TargetsInRange.RemoveAt(i);
			}
		}

		bool bModified = false;
		for (int i = 0; i < TargetsInRange.Count; ++i)
		{
			Target t = TargetsInRange[i];
			Vector2 newPos = t.HealthObj.transform.position;
			float sqDistMoved = (t.PrevPos - newPos).sqrMagnitude;
			if (sqDistMoved > 0.001f)
			{
				t.PrevPos = newPos;
				t.Dist = CalculateDistToCollider(t.Collider);
				bModified = true;
			}
		}

		if(bModified)
			TargetsInRange.OrderBy(x => x.Dist);
	}
}
