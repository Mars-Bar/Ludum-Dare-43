using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage 
	: MonoBehaviour
{
	public float Amount = 1;
	public bool DestroyOnCollision = false;
	public bool DestroyOnDamage = false;

	public GameObject RootObject;

	public HealthComponent.Team Side;

	private void DoDamage(Collider2D col)
	{
		bool bDestroy = false;
		HealthCollider healthCol = col.GetComponent<HealthCollider>();
		HealthComponent health = healthCol == null ? null : healthCol.HealthObj;
		if (health != null && health.Side != Side)
		{
			health.Health -= Amount;
			bDestroy = DestroyOnDamage;
		}
		else
		{
			bDestroy = DestroyOnCollision;
		}
		if (bDestroy)
			Destroy(RootObject == null ? gameObject : RootObject);
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		DoDamage(col);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		DoDamage(collision.collider);
	}
}
