using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwapWeapon : MonoBehaviour {
    public Image swapImage;
    public Animator animator;

    public TextMeshProUGUI ammoDisplay;
    public TextMeshProUGUI swapDisplay;

    private WeaponSpawn weaponSpawn;
    private bool playerOnCollectible = false;


    public void UpdateUI(WeaponSpawn weaponSpawn) {
        ammoDisplay.text = GameControl.control.playerAmmo.ToString("0") + " <size=\"75\">" + GameControl.control.playerWeapon.displayName;
        if (weaponSpawn != null) {
            SetNotificationText(weaponSpawn);
        }
    }


    public void Swap() {
        animator.Play("Swap", 0, 0);
        Weapon weaponToSwap = GameControl.control.playerWeapon;
        int ammoToSwap = GameControl.control.playerAmmo;
        GameControl.control.playerWeapon = weaponSpawn.weapon;
        GameControl.control.playerAmmo = weaponSpawn.ammo;
        weaponSpawn.weapon = weaponToSwap;
        weaponSpawn.ammo = ammoToSwap;
        UpdateUI(weaponSpawn);
        PrefabWeapon.SetTimer();
    }


    public void AttemptSwap() {
        if (playerOnCollectible && weaponSpawn != null) {
            Swap();
        }
    }


    public void SetNotificationText(WeaponSpawn weaponSpawn) {
        swapDisplay.text = "Swap " + GameControl.control.playerWeapon.displayName + " (" + GameControl.control.playerAmmo + ") for a " + weaponSpawn.ToString();
    }


    public void DisplayNotification(WeaponSpawn weaponSpawn) {
        SetNotificationText(weaponSpawn);
        swapDisplay.enabled = true;
    }


    public void HideNotification() {
        swapDisplay.enabled = false;
    }


    void OnTriggerEnter2D (Collider2D hitInfo) {
		GameObject collider = hitInfo.gameObject;
		if (collider.tag == "Collectible") {
			playerOnCollectible = true;
			weaponSpawn = collider.GetComponentInParent<WeaponSpawn>();
            DisplayNotification(weaponSpawn);
		}
	}


    void OnTriggerExit2D (Collider2D hitInfo) {
		GameObject collider = hitInfo.gameObject;
		if (collider.tag == "Collectible") {
			playerOnCollectible = false;
			weaponSpawn = null;
            HideNotification();
		}
	}
}
