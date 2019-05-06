using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

	public float speed = 15f;
	public int damage = 100;
	public float blastRadius = 5f;
	public float delay = 5f;
	public Rigidbody2D rb;
	public GameObject impactEffect;
	
	float countdown;
	bool hasExploded = false;

	// Use this for initialization
	void Start () {
		if (Input.touchCount > 0) {
			foreach (Touch touch in Input.touches) {
				if (touch.phase == TouchPhase.Ended) {
					countdown = delay;
					Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
					touchPosition.z = 0f;
					float deltaY = touchPosition.y - rb.position.y;
					float deltaX = touchPosition.x - rb.position.x;
					float theta = Mathf.Atan(deltaY / deltaX);
					Vector2 targetVelocity = new Vector2(speed * Mathf.Cos(theta), deltaY);
					rb.velocity = targetVelocity;
				}
			}
		}
	}

	void Explode() {
		GameObject impactEffectClone = Instantiate(impactEffect, transform.position, transform.rotation);
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, blastRadius);
		foreach (Collider2D nearbyObject in colliders) {
			Enemy enemy = nearbyObject.GetComponent<Enemy>();
			if (enemy != null) {
				enemy.TakeDamage(damage);
			}
		}
		Destroy(gameObject);
		Destroy(impactEffectClone, 5);
	}

	// Update is called once per frame
	void Update () {
		countdown = countdown - Time.deltaTime;
		if (countdown <= 0f && !hasExploded) {
			Explode();
			hasExploded = true;
		}
	}

}
