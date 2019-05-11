using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public Rigidbody2D rb;
	public GameObject impactEffect;

	// Use this for initialization
	void Start () {
		if (Input.touchCount > 0) {
			foreach (Touch touch in Input.touches) {
				if (touch.phase == TouchPhase.Ended) {
					Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
					touchPosition.z = 0f;
					float deltaY = touchPosition.y - rb.position.y;
					float deltaX = touchPosition.x - rb.position.x;
					float theta = Mathf.Atan(deltaY / deltaX);
					Vector2 targetVelocity = new Vector2(GameControl.control.playerWeapon.speed * Mathf.Cos(theta), GameControl.control.playerWeapon.speed * Mathf.Sin(theta));
					rb.velocity = targetVelocity;
				}
			}
		}
	}

	void OnTriggerEnter2D (Collider2D hitInfo) {
		Destroy(gameObject);
		Enemy enemy = hitInfo.GetComponent<Enemy>();
		if (enemy != null) {
			enemy.TakeDamage(GameControl.control.playerWeapon.damage);
		}

		GameObject impactEffectClone = Instantiate(impactEffect, transform.position, transform.rotation);
		Destroy(impactEffectClone, 5);
	}

}
