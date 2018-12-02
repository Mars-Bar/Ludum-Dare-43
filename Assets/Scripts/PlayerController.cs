using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController 
	: MonoBehaviour
{
	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	public float MoveSpeed = 1f;

	public Gun ActiveGun;

	private Vector2 _moveInput;
	private Vector2 _mousePosition;

	private Rigidbody2D _rigidbody;

	private bool CanMove()
	{
		return GameStateManager.Instance.State == GameStateManager.GameState.Moving;
	}

	private void Update ()
	{
		_moveInput = new Vector2(
			Input.GetAxis("Horizontal"),
			Input.GetAxis("Vertical"));

		_mousePosition = HelperFuncs.GetMousePositionWorld();
	}

	private void FixedUpdate()
	{
		DoAiming();
		DoMovement();
		DoShooting();
	}

	private void DoAiming()
	{
		Vector2 dirToMousePos = _mousePosition - (Vector2)transform.position;

		_rigidbody.rotation = Vector2.SignedAngle(Vector2.up, dirToMousePos);
	}

	private void DoMovement()
	{
		if (!CanMove())
			return;

		_rigidbody.AddForce(_moveInput * MoveSpeed);
	}

	private void DoShooting()
	{
		if (ActiveGun == null)
			return;

		if(Input.GetButton("Fire1"))
		{
			ActiveGun.Shoot();
		}
	}
}
