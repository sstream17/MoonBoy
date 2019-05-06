using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class PrefabWeapon : MonoBehaviour {

	public TextMeshProUGUI ammoDisplay;
	public TextMeshProUGUI grenadeDisplay;

	public Transform firePoint;
	public GameObject bulletPrefab;
	public GameObject grenadePrefab;
	public bool isGrenade = false;

	public int ammo = 50;
	public int grenades = 3;


	// Start is called before the first frame update
	void Start() {
		ammoDisplay.text = ammo.ToString("0");
		grenadeDisplay.text = grenades.ToString("0");
	}


	public void Shoot() {
		gameObject.GetComponentInParent<PlayerMovement>().timeSinceLastMove = 0;
		if (!isGrenade && ammo > 0) {
			ammo = ammo - 1;
			GameObject bulletClone = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
			Destroy(bulletClone, 10);
		}
		else if (isGrenade && grenades > 0){
			grenades = grenades - 1;
			GameObject grenadeClone = Instantiate(grenadePrefab, firePoint.position, transform.rotation);
			Destroy(grenadeClone, 10);
		}
		ammoDisplay.text = ammo.ToString("0");
		grenadeDisplay.text = grenades.ToString("0");
	}
}
