using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    Weapon weapon;
	public Rigidbody2D rb;
	public GameObject impactEffect;

	// Use this for initialization
	void Start () {
        weapon = GetComponentInParent<EnemyWeapon>().weapon;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float deltaY = player.transform.position.y - rb.position.y;
        float deltaX = player.transform.position.x - rb.position.x;
        float theta = Mathf.Atan(deltaY / deltaX);
        Vector2 targetVelocity = new Vector2(-weapon.speed * Mathf.Cos(theta), -weapon.speed * Mathf.Sin(theta));
        rb.velocity = targetVelocity;
	}

	void OnTriggerEnter2D (Collider2D hitInfo) {
		Player player = hitInfo.GetComponent<Player>();
		if (player != null) {
			player.TakeDamage(weapon.damage);
		}

		GameObject impactEffectClone = Instantiate(impactEffect, transform.position, transform.rotation);

		Destroy(gameObject);
		Destroy(impactEffectClone, 5);
	}
}
