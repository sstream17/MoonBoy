using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class TurretAI : MonoBehaviour {
	public Transform firePoint;
    public Transform target;
    public GameObject bulletPrefab;

    public float rotationSpeed = 1f;
    public float startingDistance;

	private bool allowShoot = true;
    private bool searchingForPlayer = false;
    private LayerMask layerMask;

    private Timer shootTimer;


	private void HandleTimer(System.Object source, ElapsedEventArgs e) {
		allowShoot = true;
    }


	public void SetTimer() {
		shootTimer = new Timer(3f * 1000f);
		shootTimer.Elapsed += HandleTimer;
		shootTimer.AutoReset = false;
	}


	void Shoot() {
		GameObject bulletClone = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
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


    void AttemptShot() {
        RaycastHit2D hitInfo = Physics2D.Linecast(firePoint.position, target.position, layerMask);
        if (hitInfo && (firePoint.position - target.position).magnitude <= startingDistance) {
            Player player = hitInfo.transform.GetComponent<Player>();
            if (player != null) {
				allowShoot = false;
                Shoot();
                shootTimer.Start();
			}
        }
    }


    void Start() {
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


	void FixedUpdate() {
		if (target == null) {
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }
        bool playerInFront = target.position.x < firePoint.position.x;
        Vector2 direction = new Vector2(0f, 0f);
        if (Mathf.Abs(transform.position.x - target.position.x) <= startingDistance && playerInFront) {
            direction = transform.position - target.position;
        }
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.fixedDeltaTime);
		if (allowShoot && playerInFront) {
			AttemptShot();
		}
	}
}
