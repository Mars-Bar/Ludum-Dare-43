using UnityEngine;
using System.Collections;

public class HealthBarManager 
	: MonoBehaviour
{
	public UI_HealthBar Prefab;

	public void CreateHealthBar(HealthComponent health)
	{
		UI_HealthBar healthBar = Instantiate(Prefab, transform);
		healthBar.gameObject.name = string.Format("Health Bar ({0})", health.gameObject.name);
		healthBar.HealthObj = health;

		UI_Follow follow = healthBar.GetComponent<UI_Follow>();
		follow.Target = health.transform;
	}
}
