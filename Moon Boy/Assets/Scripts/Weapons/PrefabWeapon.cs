using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class PrefabWeapon : MonoBehaviour {

	public TextMeshProUGUI ammoDisplay;
	public TextMeshProUGUI grenadeDisplay;

	public Weapon weapon;

	public Transform firePoint;
	public GameObject grenadePrefab;
	public bool isGrenade = false;

	public int grenades = 3;

	private float timeSinceLastShot;


	// Start is called before the first frame update
	void Start() {
		timeSinceLastShot = weapon.fireRate;
		ammoDisplay.text = weapon.ammo.ToString("0");
		grenadeDisplay.text = grenades.ToString("0");
	}

	void Update() {
		timeSinceLastShot = timeSinceLastShot + Time.deltaTime;
	}


	public void Shoot() {
		gameObject.GetComponentInParent<PlayerMovement>().timeSinceLastMove = 0;
		if (!isGrenade && weapon.ammo > 0 && (weapon.fireRate / timeSinceLastShot) < 1f) {
			timeSinceLastShot = 1f;
			weapon.ammo = weapon.ammo - 1;
			GameObject bulletClone = Instantiate(weapon.bulletPrefab, firePoint.position, firePoint.rotation, transform);
			Destroy(bulletClone, 10);
		}
		else if (isGrenade && grenades > 0){
			grenades = grenades - 1;
			GameObject grenadeClone = Instantiate(grenadePrefab, firePoint.position, transform.rotation);
			Destroy(grenadeClone, 10);
		}
		ammoDisplay.text = weapon.ammo.ToString("0");
		grenadeDisplay.text = grenades.ToString("0");
	}
}
