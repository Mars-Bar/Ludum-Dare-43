using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponent : MonoBehaviour
{
	public float DPS
	{
		get
		{
			if (MyWeapon == null)
				return 1;
			return MyWeapon.DmgPerSec;
		}
	}

	public HealthComponent.Team Side;

	public Aiming MyAiming;
	public Weapon MyWeapon;
	public AttackRange RangeComponent;

	public bool IsValidTarget(HealthComponent other)
	{
		if (other == null || other.Side == this.Side)
			return false;

		bool bAttack = true;

		MoveToGridObject move = GetComponent<MoveToGridObject>();
		if(move != null && move.Target != null)
		{
			// check if we have reached the thing we are attacking
			//bAttack = move.Target.gameObject == other.gameObject;
		}

		return bAttack;
	}

	private bool Attack(HealthComponent other)
	{
		if (MyWeapon == null)
			return false;

		if (!IsValidTarget(other))
			return false;

		if (MyAiming != null)
		{
			if(MyAiming.Target != other.transform)
				MyAiming.Target = other.transform;

			MyAiming.ProjectileSpeed = MyWeapon.ProjectileSpeed;

			if (!MyAiming.OnTarget)
				return false;
		}

		MyWeapon.Shoot(Side);
		return true;
	}

	private void Update()
	{
		if (RangeComponent.TargetsInRange.Count == 0)
			return;

		int maxNumTargets = 1;
		int numAttacked = 0;

		foreach(var target in RangeComponent.TargetsInRange)
		{
			if(Attack(target.HealthObj))
			{
				if (++numAttacked >= maxNumTargets)
					break;
			}
		}
	}
}
