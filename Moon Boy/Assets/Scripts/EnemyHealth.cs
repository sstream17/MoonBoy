using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public int health = 100;

	public GameObject deathEffect;
	

	public void TakeDamage (int damage) {
		health -= damage;

		if (health <= 0) {
			Die();
		}
	}

	void Die () {
		Object deathEffectClone = Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
		Destroy(deathEffectClone, 5);
	}

}
