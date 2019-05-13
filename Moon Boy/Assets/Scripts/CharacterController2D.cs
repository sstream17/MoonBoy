using UnityEngine;
using UnityEngine.Events;
using System;

public class CharacterController2D : MonoBehaviour {
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching
	[SerializeField] private Collider2D m_CrouchEnableCollider;					// A collider that will be enabled when crouching


	const float k_GroundedRadius = 0.3f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = 0.2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;


	private void Awake() {
		m_CrouchEnableCollider.enabled = false;
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null) {
			OnLandEvent = new UnityEvent();
		}

		if (OnCrouchEvent == null) {
			OnCrouchEvent = new BoolEvent();
		}
	}


	private void FixedUpdate() {
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders[i].gameObject != gameObject) {
				m_Grounded = true;
				if (!wasGrounded) {
					OnLandEvent.Invoke();
				}
			}
		}
	}


	public void Move(float horizontalMove, float verticalMove, bool crouch, bool canFly) {
		// If crouching, check to see if the character can stand up
		if (!crouch) {
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround)) {
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl) {
			// If crouching
			if (crouch) {
				if (!m_wasCrouching) {
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null) {
					m_CrouchDisableCollider.enabled = false;
					m_CrouchEnableCollider.enabled = true;
				}
			}
			else {
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null) {
					m_CrouchDisableCollider.enabled = true;
					m_CrouchEnableCollider.enabled = false;
				}

				if (m_wasCrouching) {
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(horizontalMove * 10f, m_Rigidbody2D.velocity.y);
			// If the character is in the air, maintain current trajectory
			if (!m_Grounded && verticalMove <= 0) {
				targetVelocity = new Vector2(m_Rigidbody2D.velocity.x, m_Rigidbody2D.velocity.y + verticalMove);
			}
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
		}
		// If the player should jump...
		if (m_Grounded && verticalMove > 0) {
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
		else if (verticalMove > 0 && canFly) {
			m_Grounded = false;
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, verticalMove);
		}
	}
}
