using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleWeapon : MonoBehaviour {
    public Image grenadeImage;
    public Image ammoImage;

    public void Toggle() {
        PrefabWeapon weapon = gameObject.GetComponentInChildren<PrefabWeapon>();
        if (grenadeImage.enabled) {
            grenadeImage.enabled = false;
            ammoImage.enabled = true;
            weapon.isGrenade = true;
        }
        else {
            ammoImage.enabled = false;
            grenadeImage.enabled = true;
            weapon.isGrenade = false;
        }
    }
}
