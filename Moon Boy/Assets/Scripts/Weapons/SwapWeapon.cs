using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapWeapon : MonoBehaviour {
    public Image swapImage;
    public Animator animator;

    private WeaponSpawn weaponSpawn;
    private bool playerOnCollectible = false;


    public void Swap() {
        animator.Play("Swap", 0, 0);
        GameControl.control.playerWeapon = weaponSpawn.weapon;
        PrefabWeapon.SetTimer();
    }


    public void AttemptSwap() {
        if (playerOnCollectible && weaponSpawn != null) {
            Swap();
        }
    }


    void OnTriggerEnter2D (Collider2D hitInfo) {
		GameObject collider = hitInfo.gameObject;
		if (collider.tag == "Collectible") {
			playerOnCollectible = true;
			weaponSpawn = collider.GetComponentInParent<WeaponSpawn>();
		}
	}


    void OnTriggerExit2D (Collider2D hitInfo) {
		GameObject collider = hitInfo.gameObject;
		if (collider.tag == "Collectible") {
			playerOnCollectible = false;
			weaponSpawn = null;
		}
	}
}
