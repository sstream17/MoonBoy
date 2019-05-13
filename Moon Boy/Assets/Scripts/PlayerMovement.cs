using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public Animator animator;
	public Joystick joystick;
	public Transform firePoint;
	public TextMeshProUGUI energyDisplay;

	public float runSpeed = 40f;
	public float flyingSpeed = 0.5f;
	public float timeSinceLastMove = 0f;
	public float energy = 100f;

	float horizontalMove = 0f;
	float verticalMove = 0f;
	float verticalVelocity = 0f;

	[HideInInspector]
	public bool crouch = false;
	bool canFly = true;

	// Update is called once per frame
	void Update () {
		float energyModifier = 0f;

		energyDisplay.text = energy.ToString("0");
		if (energy <= 0) {
			canFly = false;
		}
		else if (energy >= 100) {
			canFly = true;
			energy = 100;
		}
		else {
			canFly = true;
		}

		firePoint.localPosition = new Vector2(1.07f, 0.181f);
		timeSinceLastMove = timeSinceLastMove + Time.deltaTime;

		horizontalMove = joystick.Horizontal * runSpeed;
		animator.SetFloat("Speed", horizontalMove);

		verticalMove = joystick.Vertical;
		animator.SetFloat("VerticalSpeed", verticalMove);

		if (horizontalMove != 0 || verticalMove != 0) {
			timeSinceLastMove = 0;
		}

		// If the player is flying, compute the vertical velocity and deplete the energy
		if (verticalMove >= 0.2f && energy > 0f) {
			verticalVelocity = verticalMove * flyingSpeed;
			energyModifier = -verticalMove * 0.3f;
			animator.SetBool("IsFlying", true);
		}
		// If the player jumps
		else if (verticalMove >= 0.2f) {
			verticalVelocity = verticalMove * flyingSpeed;
			animator.SetBool("IsFlying", true);
		}
		else if (verticalMove <= -0.2f) {
			horizontalMove = 0;
			verticalVelocity = -50f;
			crouch = true;
			energyModifier = canFly ? 0.02f : 0f;
			firePoint.localPosition = new Vector2(1.07f, -0.66f);
		}
		else if (Mathf.Abs(horizontalMove) > 2) {
			verticalVelocity = 0;
			crouch = false;
			energyModifier = canFly ? 0.01f : 0f;
		}
		else if (horizontalMove == 0 && verticalMove == 0) {
			verticalVelocity = 0;
			crouch = false;
			energyModifier = canFly ? 0.04f : 0f;
		}

		if (timeSinceLastMove > 10) {
			energyModifier = canFly ? 0.2f : 0f;
			animator.SetBool("IsIdle", true);
		}
		else {
			animator.SetBool("IsIdle", false);
		}

		energy = energy + energyModifier;
	}


	public void OnLanding () {
		animator.SetBool("IsFlying", false);
		canFly = true;
		if (energy <= 0) {
			energy = 1f;
		}
	}


	public void OnCrouching (bool isCrouching) {
		animator.SetBool("IsCrouching", isCrouching);
	}


	void FixedUpdate () {
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, verticalVelocity * Time.fixedDeltaTime, crouch, canFly);
	}
}
