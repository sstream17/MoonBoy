using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject {

	public int ammo = 50;
    public int damage;
	public float speed;
    public int fireRate;
    public bool canRapidFire;

    public GameObject bulletPrefab;
}
