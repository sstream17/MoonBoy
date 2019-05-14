using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public int health = 100;
	public float blastRadius;
	public float explosionDelay;
	public int explosionDamage;

	public GameObject deathEffect;


	public void TakeDamage (int damage) {
		health -= damage;

		if (health <= 0) {
			Die();
		}
	}


	void Die () {
		EnemyWeapon weapon = gameObject.GetComponentInChildren<EnemyWeapon>();
		Destroy(gameObject);
		if (weapon != null) {
			GameObject weaponSpawn = Instantiate(weapon.weapon.spawnPrefab, transform.position, Quaternion.identity);
			weaponSpawn.GetComponent<WeaponSpawn>().Initialize(weapon.weapon);
		}
		Object deathEffectClone = Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(deathEffectClone, 5);
	}


	IEnumerator SelfDestruct() {
		yield return new WaitForSeconds(explosionDelay);
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, blastRadius);
		foreach (Collider2D nearbyObject in colliders) {
			Player player = nearbyObject.GetComponent<Player>();
			if (player != null) {
				player.TakeDamage(explosionDamage);
			}
		}
		Die();
		yield return false;
	}


	void OnTriggerEnter2D (Collider2D hitInfo) {
		Player player = hitInfo.GetComponent<Player>();
		if (player != null) {
			StartCoroutine(SelfDestruct());
		}
	}

}
