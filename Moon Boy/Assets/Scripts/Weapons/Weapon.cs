using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject {

	public string displayName;
    public int damage;
	public float speed;
    public float fireRate;
    public bool canRapidFire;

    public GameObject bulletPrefab;
	public GameObject enemyBulletPrefab;
	public GameObject spawnPrefab;
}
