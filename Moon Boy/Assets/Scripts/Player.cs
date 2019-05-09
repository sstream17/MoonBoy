using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour {

    public int health = 100;
    public float damageModifier = 0.25f;

	public GameObject deathEffect;


	public void TakeDamage (int damage) {
		health -= (int) Mathf.Ceil(damage * damageModifier);
        GameControl.control.healthDisplay.text = health.ToString("0");

		if (health <= 0) {
            health = 0;
            GameControl.control.healthDisplay.text = health.ToString("0");
			GameControl.control.KillPlayer(this);
		}
	}
}
