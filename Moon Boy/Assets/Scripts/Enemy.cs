using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public int health = 100;
	public float speed;
	public float startingDistance;
	public float stoppingDistance;
	public float retreatDistance;

	public Transform player;

	public GameObject deathEffect;


	void Start() {
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}


	void Update() {
		float distance = Vector2.Distance(transform.position, player.position);
		if (distance < startingDistance && distance > stoppingDistance) {
			transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
		}
		else if (distance < stoppingDistance && distance > retreatDistance) {
			transform.position = this.transform.position;
		}
		else if (distance < retreatDistance) {
			transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
		}
	}

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
