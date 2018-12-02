using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent
	: MonoBehaviour, IHealth
{
	protected virtual void Awake()
	{
		HealthBarManager hbm = FindObjectOfType<HealthBarManager>();
		if(hbm)
		{
			hbm.CreateHealthBar(this);
		}
	}

	public float Health
	{
		get { return _health; }
		set
		{
			_health = value;
			if (OnHealthChanged != null)
				OnHealthChanged();

			if (_health <= 0)
			{
				Death();
			}
		}
	}
	public float MaxHealth
	{
		get { return _maxHealth; }
		set { _maxHealth = value; }
	}

	public delegate void HealthChangeDelegate();
	public event HealthChangeDelegate OnHealthChanged;

	[SerializeField]
	private float _health = 10;
	[SerializeField]
	private float _maxHealth = 10;

	public enum Team
	{
		Friend,
		Enemy,
	}
	public Team Side;

	protected virtual void Death()
	{
		Destroy(gameObject);
	}
}
