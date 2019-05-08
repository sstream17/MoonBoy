using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	Weapon weapon;
	public Rigidbody2D rb;
	public GameObject impactEffect;

	// Use this for initialization
	void Start () {
		if (Input.touchCount > 0) {
			foreach (Touch touch in Input.touches) {
				if (touch.phase == TouchPhase.Ended) {
					weapon = GetComponentInParent<PrefabWeapon>().weapon;
					Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
					touchPosition.z = 0f;
					float deltaY = touchPosition.y - rb.position.y;
					float deltaX = touchPosition.x - rb.position.x;
					float theta = Mathf.Atan(deltaY / deltaX);
					Vector2 targetVelocity = new Vector2(weapon.speed * Mathf.Cos(theta), weapon.speed * Mathf.Sin(theta));
					rb.velocity = targetVelocity;
				}
			}
		}
	}

	void OnTriggerEnter2D (Collider2D hitInfo) {
		Enemy enemy = hitInfo.GetComponent<Enemy>();
		if (enemy != null) {
			enemy.TakeDamage(weapon.damage);
		}

		GameObject impactEffectClone = Instantiate(impactEffect, transform.position, transform.rotation);

		Destroy(gameObject);
		Destroy(impactEffectClone, 5);
	}

}
