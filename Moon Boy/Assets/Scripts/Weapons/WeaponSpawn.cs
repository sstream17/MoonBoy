﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawn : MonoBehaviour
{
    public Weapon weapon;
    public int ammo;


    public void Initialize(Weapon weaponToSpawn) {
        weapon = weaponToSpawn;
        int newAmmo = (int) Mathf.Floor(Random.Range(0f, 50f));
        ammo = newAmmo;
    }


    public override string ToString() {
        return weapon.displayName + " (" + ammo + ")";
    }
}
