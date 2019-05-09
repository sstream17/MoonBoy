using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class PrefabWeapon : MonoBehaviour {

	public Transform firePoint;
	public GameObject grenadePrefab;
	public bool isGrenade = false;

	private bool hasShot = false;
	private static bool allowShoot = true;
	private static Timer timer;

	public int grenades = 3;


	private static void HandleTimer(System.Object source, ElapsedEventArgs e) {
		allowShoot = true;
    }


	public static void SetTimer() {
		timer = new Timer(GameControl.control.playerWeapon.fireRate * 1000f);
		timer.Elapsed += HandleTimer;
		timer.AutoReset = false;
	}


	void Shoot() {
		gameObject.GetComponentInParent<PlayerMovement>().timeSinceLastMove = 0;
		GameControl.control.playerWeapon.ammo = GameControl.control.playerWeapon.ammo - 1;
		GameObject bulletClone = Instantiate(GameControl.control.playerWeapon.bulletPrefab, firePoint.position, firePoint.rotation, transform);
		Destroy(bulletClone, 10);
		GameControl.control.ammoDisplay.text = GameControl.control.playerWeapon.ammo.ToString("0");
		timer.Start();
	}


	void ThrowGrenade() {
		gameObject.GetComponentInParent<PlayerMovement>().timeSinceLastMove = 0;
		grenades = grenades - 1;
		GameObject grenadeClone = Instantiate(grenadePrefab, firePoint.position, transform.rotation);
		Destroy(grenadeClone, 10);
		GameControl.control.grenadeDisplay.text = grenades.ToString("0");
	}


	public void SendShoot() {
		hasShot = true;
	}


	// Start is called before the first frame update
	void Start() {
		PrefabWeapon.SetTimer();
		GameControl.control.ammoDisplay.text = GameControl.control.playerWeapon.ammo.ToString("0");
		GameControl.control.grenadeDisplay.text = grenades.ToString("0");
	}


	void Update() {
		if (hasShot && allowShoot && !isGrenade && GameControl.control.playerWeapon.ammo > 0) {
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
