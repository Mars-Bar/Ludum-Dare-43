using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon 
	: MonoBehaviour
{
	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
		_soundClipManager = FindObjectOfType<SoundClipManager>();
	}

	public Transform BulletParent;
	public Transform BulletSpawnPos;
	public Projectile ProjectilePrefab;

	private AudioSource _audioSource;

	public bool HasAmmo = true;
	public int AmmoLeft = 100;
	
	public float ProjectileSpeed = 15;
	public float FireCooldown = 0.1f;
	public float DmgPerSec = 1f;

	private float _lastFireTime = 0f;

	public SoundClipManager.SoundID SoundEffectID;
	SoundClipManager _soundClipManager;

	public virtual bool CanShoot()
	{
		if ((AmmoLeft <= 0) || (ProjectilePrefab == null))
			return false;

		if (GameStateManager.Instance.State != GameStateManager.GameState.Moving)
			return false;

		// check gun cooldown
		return _lastFireTime + FireCooldown < Time.time;
	}

	public void Shoot(HealthComponent.Team mySide)
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
		Projectile newBullet = Instantiate(ProjectilePrefab, spawnPosition, spawnRotation, BulletParent);

		Damage dmg = newBullet.GetComponentInChildren<Damage>();
		dmg.Amount = DmgPerSec * FireCooldown;
		dmg.Side = mySide;

		// Fire Bullet
		FireBullet(newBullet);

		if (_audioSource != null)
		{
			// Play sound
			AudioClip clip = _soundClipManager.GetRandomSoundEffect(SoundEffectID);
			if(clip != null)
				_audioSource.PlayOneShot(clip);
		}


		// Spend Ammo
		--AmmoLeft;
	}

	private void FireBullet(Projectile bullet)
	{
		Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
		if (!bulletRb)
			return;

		// Fire bullet
		bulletRb.velocity = ProjectileSpeed * bullet.transform.up;

		_lastFireTime = Time.time;
	}
}
