using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    public Rigidbody2D rb;
	public GameObject impactEffect;
    public float damage;
    public float speed;

	// Use this for initialization
	void Start () {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        bool isCrouching = player.gameObject.GetComponent<PlayerMovement>().crouch;
        float yOffset = isCrouching ? -0.6f : 0.4f;
        float deltaY = player.transform.position.y - rb.position.y + yOffset;
        float deltaX = player.transform.position.x - rb.position.x;
        float theta = Mathf.Atan(deltaY / deltaX);
        Vector2 targetVelocity = new Vector2(-speed * Mathf.Cos(theta), -speed * Mathf.Sin(theta));
        rb.velocity = targetVelocity;
	}

	void OnTriggerEnter2D (Collider2D hitInfo) {
        Destroy(gameObject);
		Player player = hitInfo.GetComponent<Player>();
		if (player != null) {
			player.TakeDamage((int) Mathf.Floor(damage));
		}

		GameObject impactEffectClone = Instantiate(impactEffect, transform.position, transform.rotation);
		Destroy(impactEffectClone, 5);
	}
}
