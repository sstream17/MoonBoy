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
		GameControl.control.playerAmmo = GameControl.control.playerAmmo - 1;
		GameObject bulletClone = Instantiate(GameControl.control.playerWeapon.bulletPrefab, firePoint.position, firePoint.rotation);
		Destroy(bulletClone, 10);
		ammoDisplay.text = GameControl.control.playerAmmo.ToString("0") + "<size=\"45\"> " + GameControl.control.playerWeapon.displayName;
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
		ammoDisplay.text = GameControl.control.playerAmmo.ToString("0") + "<size=\"45\"> " + GameControl.control.playerWeapon.displayName;
		grenadeDisplay.text = grenades.ToString("0");
	}


	void Update() {
		if (hasShot && allowShoot && !isGrenade && GameControl.control.playerAmmo > 0) {
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
