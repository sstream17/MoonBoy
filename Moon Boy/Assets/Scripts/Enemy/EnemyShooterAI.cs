using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Seeker))]
public class EnemyShooterAI : MonoBehaviour
{
    public GameObject originalLocation;
    private float distanceFromStart;
    public float maximumDistance;
    private bool returning = false;
    private bool avoidingGround = false;
    public Transform target;
    public Transform player;
    public Transform retreatPoint;
    public float startingDistance;
    public float stoppingDistance;
    public float retreatDistance;
    private bool searchingForPlayer = false;

    public float updateRate = 2f;

    private Seeker seeker;
    private Rigidbody2D rb;

    public Animator animator;
    [SerializeField] private LayerMask m_WhatIsGround;
	[SerializeField] private Transform m_GroundCheck;
    const float k_GroundedRadius = 0.3f;
    private bool m_Grounded;
    private bool movingForward;
    private bool movingBackward;

    public Path path;

    public float speed = 100f;
    public ForceMode2D forceMode;

    public bool moveUpwards = false;

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


    IEnumerator SearchForPlayer() {
        GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
        if (searchResult == null) {
            yield return new WaitForSeconds(1f);
            StartCoroutine(SearchForPlayer());
        }
        else {
            searchingForPlayer = false;
            player = searchResult.transform;
            target = player;
            StartCoroutine(UpdatePath());
            yield break;
        }
    }


    IEnumerator UpdatePath() {
        if (target == null) {
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            yield break;
        }
        if (seeker.IsDone()) {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }

        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());
    }


    IEnumerator AvoidGround() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, 4f, m_WhatIsGround);
		foreach (Collider2D collider in colliders) {
			if (collider.gameObject != gameObject) {
				rb.AddForce(Vector2.up * speed * Time.fixedDeltaTime, forceMode);
			}
		}
        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(AvoidGround());
    }


    void OnLanding() {
        m_Grounded = true;
    }


    void SetAnimator(bool grounded, bool movingForward, bool movingBackward) {
        animator.SetBool("Grounded", grounded);
        animator.SetBool("MovingForward", movingForward);
        animator.SetBool("MovingBackward", movingBackward);
    }


    void Start() {
        retreatPoint = GameObject.FindGameObjectWithTag("RetreatPoint").transform;
        originalLocation = new GameObject("OriginalLocation");
        originalLocation.transform.position = transform.position;
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

        movingForward = false;
        movingBackward = false;
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

        if (currentWaypoint >= path.vectorPath.Count) {
            if (pathHasEnded) {
                return;
            }
            pathHasEnded = true;
            return;
        }
        pathHasEnded = false;

        float distanceToPlayer = Mathf.Abs(transform.position.x - player.position.x);
        bool playerInFront = player.position.x < transform.position.x;

        Vector2 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        direction = direction * speed * Time.fixedDeltaTime;

        distanceFromStart = transform.position.x - originalLocation.transform.position.x;
        if (distanceFromStart < -maximumDistance) {
            returning = true;
            target = retreatPoint;
        }

        if (!returning) {
            if (distanceToPlayer < startingDistance && moveUpwards) {
                rb.AddForce(Vector2.up * speed * Time.fixedDeltaTime, forceMode);
                return;
            }

            if (distanceToPlayer > stoppingDistance && distanceToPlayer < startingDistance) {
                movingForward = direction.x < -15f ? true : false;
                rb.AddForce(direction, forceMode);
            }
            else if (distanceToPlayer < stoppingDistance && distanceToPlayer > retreatDistance) {
                movingForward = false;
                movingBackward = false;
                transform.position = this.transform.position;
            }
            else if (distanceToPlayer < retreatDistance) {
                returning = true;
                target = retreatPoint;
                movingBackward = -direction.x > 10f ? true : false;
            }
        }
        else {
            movingBackward = true;
            if (!avoidingGround) {
                avoidingGround = true;
                StartCoroutine(AvoidGround());
            }
            if (distanceToPlayer < startingDistance && moveUpwards) {
                rb.AddForce(Vector2.up * speed * Time.fixedDeltaTime, forceMode);
                return;
            }

            if (distanceToPlayer > stoppingDistance && playerInFront) {
                avoidingGround = false;
                returning = false;
                target = null;
            }
            rb.AddForce(direction * 1.4f, forceMode);
        }

        SetAnimator(m_Grounded, movingForward, movingBackward);

        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance) {
            currentWaypoint = currentWaypoint + 1;
            return;
        }
    }
}
