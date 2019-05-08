using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
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

	private bool hasShot = false;
	private bool allowShoot = true;
	private Timer timer;

	public int grenades = 3;


	private void HandleTimer(System.Object source, ElapsedEventArgs e) {
		allowShoot = true;
    }


	public void SetTimer() {
		timer = new Timer(weapon.fireRate * 1000f);
		timer.Elapsed += HandleTimer;
		timer.AutoReset = false;
	}


	void Shoot() {
		gameObject.GetComponentInParent<PlayerMovement>().timeSinceLastMove = 0;
		weapon.ammo = weapon.ammo - 1;
		GameObject bulletClone = Instantiate(weapon.bulletPrefab, firePoint.position, firePoint.rotation, transform);
		Destroy(bulletClone, 10);
		ammoDisplay.text = weapon.ammo.ToString("0");
		timer.Start();
	}


	void ThrowGrenade() {
		gameObject.GetComponentInParent<PlayerMovement>().timeSinceLastMove = 0;
		grenades = grenades - 1;
		GameObject grenadeClone = Instantiate(grenadePrefab, firePoint.position, transform.rotation);
		Destroy(grenadeClone, 10);
		grenadeDisplay.text = grenades.ToString("0");
	}


	public void SendShoot() {
		hasShot = true;
	}


	// Start is called before the first frame update
	void Start() {
		SetTimer();
		ammoDisplay.text = weapon.ammo.ToString("0");
		grenadeDisplay.text = grenades.ToString("0");
	}


	void Update() {
		if (hasShot && allowShoot && !isGrenade && weapon.ammo > 0) {
			allowShoot = false;
			hasShot = false;
			Shoot();
		}
		else if (hasShot && isGrenade && grenades > 0) {
			hasShot = false;
			ThrowGrenade();
		}
		hasShot = false;
	}
}
