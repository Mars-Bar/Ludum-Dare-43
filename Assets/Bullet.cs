﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float LifeTime = 1f;
	
	void Start ()
	{
		Destroy(gameObject, LifeTime);
	}
}
