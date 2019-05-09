using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleWeapon : MonoBehaviour {

    public void Toggle() {
        PrefabWeapon weapon = gameObject.GetComponentInChildren<PrefabWeapon>();
        if (GameControl.control.grenadeImage.enabled) {
            GameControl.control.grenadeImage.enabled = false;
            GameControl.control.ammoImage.enabled = true;
            weapon.isGrenade = true;
        }
        else {
            GameControl.control.ammoImage.enabled = false;
            GameControl.control.grenadeImage.enabled = true;
            weapon.isGrenade = false;
        }
    }
}
