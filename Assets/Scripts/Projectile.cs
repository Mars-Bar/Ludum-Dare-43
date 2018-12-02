using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float LifeTime = 1f;
	
	void Start ()
	{
		Destroy(gameObject, LifeTime);
	}
}
