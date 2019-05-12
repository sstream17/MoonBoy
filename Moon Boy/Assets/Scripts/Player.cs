using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour {

    public TextMeshProUGUI healthDisplay;
    public int health = 100;

	public GameObject deathEffect;


    public void UpdateUI() {
        healthDisplay.text = health.ToString("0");
    }

	public void TakeDamage (int damage) {
		health -= damage;
        UpdateUI();

		if (health <= 0) {
            health = 0;
            UpdateUI();
			GameControl.control.KillPlayer(this);
		}
	}
}
