using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour {
	public Weapon weapon;
	public Transform firePoint;
    public Transform target;
	private EnemyShooterAI enemy;

    [HideInInspector]
    public float startingDistance;

	private bool allowShoot = true;
    private bool searchingForPlayer = false;
    private LayerMask layerMask;

    private Timer shootTimer;


	private void HandleTimer(System.Object source, ElapsedEventArgs e) {
		allowShoot = true;
    }


	public void SetTimer() {
		shootTimer = new Timer(weapon.fireRate * 1.5f * 1000f);
		shootTimer.Elapsed += HandleTimer;
		shootTimer.AutoReset = false;
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
            yield break;
        }
    }


	IEnumerator WaitToShoot() {
		yield return new WaitForSeconds(0.1f);
		enemy.moveUpwards = false;
		Shoot();
		shootTimer.Start();
		yield break;
	}


    void AttemptShot() {
        RaycastHit2D hitInfo = Physics2D.Linecast(firePoint.position, target.position, layerMask);
        if (hitInfo && (firePoint.position - target.position).magnitude <= startingDistance) {
            Player player = hitInfo.transform.GetComponent<Player>();
            if (player != null && enemy.moveUpwards) {
				allowShoot = false;
				StartCoroutine(WaitToShoot());
            }
			else if (player != null) {
				allowShoot = false;
                Shoot();
                shootTimer.Start();
			}
			else {
				enemy.moveUpwards = true;
			}
        }
    }


    void Start() {
		int randomWeaponIndex = (int) Mathf.Floor(Random.value * GameControl.control.enemyWeapons.Length);
		weapon = GameControl.control.enemyWeapons[randomWeaponIndex];
        enemy = GetComponentInParent<EnemyShooterAI>();
		startingDistance = enemy.startingDistance;
        SetTimer();
        layerMask = LayerMask.GetMask("Player", "Ground", "Enemy", "Turret");
        if (target == null) {
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }
	}


	void FixedUpdate() {
		if (target == null) {
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }
        bool playerInFront = target.position.x < firePoint.position.x;
		if (allowShoot && playerInFront) {
			AttemptShot();
		}
	}
}
