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
    private RestockPoint restockPoint;
    private bool playerOnCollectible = false;


    public void UpdateUI(WeaponSpawn weaponSpawn) {
        ammoDisplay.text = GameControl.control.playerAmmo.ToString("0") + "<size=\"45\"> " + GameControl.control.playerWeapon.displayName;
        if (weaponSpawn != null) {
            SetNotificationSwapText(weaponSpawn);
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


    public void Restock() {
        animator.Play("Swap", 0, 0);
        GameControl.control.playerAmmo = GameControl.control.playerAmmo + restockPoint.ammo;
        UpdateUI(weaponSpawn);
        Destroy(restockPoint.gameObject);
    }


    public void AttemptSwap() {
        if (playerOnCollectible && weaponSpawn != null) {
            Swap();
        }
        else if (playerOnCollectible && restockPoint != null) {
            Restock();
        }
    }


    public void SetNotificationSwapText(WeaponSpawn weaponSpawn) {
        swapDisplay.text = "Swap " + GameControl.control.playerWeapon.displayName + " (" + GameControl.control.playerAmmo + ") for a " + weaponSpawn.ToString();
    }


    public void DisplaySwapNotification(WeaponSpawn weaponSpawn) {
        SetNotificationSwapText(weaponSpawn);
        swapDisplay.enabled = true;
    }


    public void SetNotificationRestockText() {
        swapDisplay.text = "Restock 50 ammo";
    }


    public void DisplayRestockNotification() {
        SetNotificationRestockText();
        swapDisplay.enabled = true;
    }


    public void HideNotification() {
        swapDisplay.enabled = false;
    }


    void OnTriggerEnter2D (Collider2D hitInfo) {
		weaponSpawn = hitInfo.gameObject.GetComponentInParent<WeaponSpawn>();
        restockPoint = hitInfo.gameObject.GetComponentInParent<RestockPoint>();
		if (weaponSpawn != null) {
			playerOnCollectible = true;
            DisplaySwapNotification(weaponSpawn);
		}
        else if (restockPoint != null) {
            playerOnCollectible = true;
            DisplayRestockNotification();
        }
	}


    void OnTriggerExit2D (Collider2D hitInfo) {
		GameObject collider = hitInfo.gameObject;
		if (collider.tag == "Collectible") {
			playerOnCollectible = false;
			weaponSpawn = null;
            restockPoint = null;
            HideNotification();
		}
	}
}
