using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawn : MonoBehaviour
{
    public Weapon weapon;
    public int ammo;

    private float minimumRange = 0f;
    private float maximumRange = 50f;


    private void SetRange(Weapon weapon)
    {
        if (weapon.name.Contains("Auto Cannon"))
        {
            minimumRange = 50f;
            maximumRange = 100f;
        }
    }

    public void Initialize(Weapon weaponToSpawn) {
        weapon = weaponToSpawn;
        SetRange(weaponToSpawn);
        int newAmmo = (int) Mathf.Floor(Random.Range(minimumRange, maximumRange));
        ammo = newAmmo;
    }


    public override string ToString() {
        return weapon.displayName + " (" + ammo + ")";
    }
}
