using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour {

    public TextMeshProUGUI healthDisplay;
    public int health = 100;
    public float damageModifier = 0.25f;

	public GameObject deathEffect;


	public void TakeDamage (int damage) {
		health -= (int) Mathf.Ceil(damage * damageModifier);
        healthDisplay.text = health.ToString("0");

		if (health <= 0) {
            health = 0;
            healthDisplay.text = health.ToString("0");
			Die();
		}
	}

	void Die () {
		Object deathEffectClone = Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
		Destroy(deathEffectClone, 5);
	}
}
