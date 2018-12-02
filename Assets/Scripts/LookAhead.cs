using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAhead 
	: MonoBehaviour
{
	public Rigidbody2D Rigidbody;

	public float MinLookAhead = 0f;
	public float MaxLookAhead = 5f;

	public float VelocityMultiplier = 1f;

	private void Update()
	{
		Vector2 velocity = Rigidbody.velocity;

		float velComponent = Vector2.Dot(transform.up, velocity) * VelocityMultiplier;

		transform.localPosition = new Vector2(0f, Mathf.Lerp(MinLookAhead, MaxLookAhead, velComponent));
	}
}
