using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun 
	: MonoBehaviour
{
	public Transform BulletParent;
	public Transform BulletSpawnPos;
	public Bullet ProjectilePrefab;
	public int AmmoLeft = 100;
	public float Power = 15;
	public float FireCooldown = 0.1f;

	private float _lastFireTime = 0f;

	private bool CanShoot()
	{
		if ((AmmoLeft <= 0) || (ProjectilePrefab == null))
			return false;

		if (GameStateManager.Instance.State != GameStateManager.GameState.Moving)
			return false;

		// check gun cooldown
		return _lastFireTime + FireCooldown < Time.time;
	}

	public void Shoot()
	{
		if (!CanShoot())
			return;

		// Get spawn info
		Vector2 spawnPosition;
		Quaternion spawnRotation;

		if (BulletSpawnPos != null)
		{
			spawnPosition = BulletSpawnPos.position;
			spawnRotation = BulletSpawnPos.rotation;
		}
		else
		{
			spawnPosition = transform.position;
			spawnRotation = transform.rotation;
		}

		// Spawn bullet
		Bullet newBullet = Instantiate(ProjectilePrefab, spawnPosition, spawnRotation, BulletParent);

		// Fire Bullet
		FireBullet(newBullet);

		// Spend Ammo
		--AmmoLeft;
	}

	private void FireBullet(Bullet bullet)
	{
		Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
		if (!bulletRb)
			return;

		// Fire bullet
		bulletRb.AddForce(Power * bullet.transform.up, ForceMode2D.Impulse);

		_lastFireTime = Time.time;
	}
}
