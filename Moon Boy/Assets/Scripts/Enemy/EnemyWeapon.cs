﻿using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour {
	public Weapon weapon;
	public Transform firePoint;
    public Transform target;
    [HideInInspector]
    public float startingDistance;

	private bool allowShoot = true;
    private bool searchingForPlayer = false;
    private LayerMask layerMask;

    private Timer timer;



	private void HandleTimer(System.Object source, ElapsedEventArgs e) {
		allowShoot = true;
    }


	public void SetTimer() {
		timer = new Timer(weapon.fireRate * 1000f);
		timer.Elapsed += HandleTimer;
		timer.AutoReset = false;
	}


	void Shoot() {
		GameObject bulletClone = Instantiate(weapon.enemyBulletPrefab, firePoint.position, firePoint.rotation, transform);
		Destroy(bulletClone, 10);
	}


    IEnumerator SearchForPlayer() {
        GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
        if (searchResult == null) {
            yield return new WaitForSeconds(1f);
            StartCoroutine(SearchForPlayer());
        }
        else {
            searchingForPlayer = false;
            target = searchResult.transform;
            yield return false;
        }
    }


    void AttemptShot() {
        RaycastHit2D hitInfo = Physics2D.Linecast(firePoint.position, target.position, layerMask);
        if (hitInfo && (firePoint.position - target.position).magnitude <= startingDistance) {
            Player player = hitInfo.transform.GetComponent<Player>();
            if (player != null) {
                allowShoot = false;
                Shoot();
                timer.Start();
            }
        }
    }


    void Start() {
        startingDistance = GetComponentInParent<EnemyAI>().startingDistance;
        SetTimer();
        layerMask = LayerMask.GetMask("Player", "Ground", "Enemy");
        if (target == null) {
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }
	}


	void Update() {
        bool playerInFront = target.position.x < firePoint.position.x;
		if (allowShoot && playerInFront) {
			AttemptShot();
		}
	}
}