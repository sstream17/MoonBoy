using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Seeker))]
public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float startingDistance;
    public float stoppingDistance;
    public float retreatDistance;
    private bool searchingForPlayer = false;

    public float updateRate = 2f;

    private Seeker seeker;
    private Rigidbody2D rb;

    public bool trackMotion = false;
    public Animator animator;
    [SerializeField] private LayerMask m_WhatIsGround;
	[SerializeField] private Transform m_GroundCheck;
    const float k_GroundedRadius = 0.3f;
    private bool m_Grounded;

    public Path path;

    public float speed = 100f;
    public ForceMode2D forceMode;

    [HideInInspector]
    public bool pathHasEnded = false;

    public float nextWaypointDistance = 3f;

    private int currentWaypoint = 0;


    public void OnPathComplete(Path pathSought) {
        if (!pathSought.error) {
            path = pathSought;
            currentWaypoint = 0;
        }
    }


    IEnumerator UpdatePath() {
        if (target == null) {
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            yield return false;
        }

        seeker.StartPath(transform.position, target.position, OnPathComplete);

        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());
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
            StartCoroutine(UpdatePath());
            yield return false;
        }
    }


    void Start() {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if (target == null) {
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());
    }


    void OnLanding() {
        m_Grounded = true;
    }


    void FixedUpdate() {
        if (target == null) {
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        if (path == null) {
            return;
        }

        if (trackMotion) {
            bool wasGrounded = m_Grounded;
    		m_Grounded = false;
    		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
    		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
    		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
    		for (int i = 0; i < colliders.Length; i++) {
    			if (colliders[i].gameObject != gameObject) {
    				m_Grounded = true;
    				if (!wasGrounded) {
    					OnLanding();
    				}
    			}
    		}
        }

        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

        if (currentWaypoint >= path.vectorPath.Count) {
            if (pathHasEnded) {
                return;
            }
            pathHasEnded = true;
            return;
        }
        pathHasEnded = false;

        Vector2 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        direction = direction * speed * Time.fixedDeltaTime;

        if (distanceToPlayer > stoppingDistance && distanceToPlayer < startingDistance) {
            rb.AddForce(direction, forceMode);
        }
        else if (distanceToPlayer < stoppingDistance && distanceToPlayer > retreatDistance) {
            transform.position = this.transform.position;
        }
        else if (distanceToPlayer < retreatDistance) {
            rb.AddForce(-direction, forceMode);
        }

        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance) {
            currentWaypoint = currentWaypoint + 1;
            return;
        }
    }
}
