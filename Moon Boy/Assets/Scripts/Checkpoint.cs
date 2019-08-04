using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool searching = false;
    private int framesToWait = 750;
    private float overlapRadius = 30f;

    private void OnEnable()
    {
        searching = false;
    }

    void Start()
    {
        GameControl.control.spawnPoint.position = transform.position;
    }

    IEnumerator FindNewSpawnPoint(Vector2 potentialSpawnPosition)
    {
        bool enemyFound = false;
        searching = true;
        for (int i = 0; i < framesToWait; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, overlapRadius);
        foreach (Collider2D nearbyObject in colliders)
        {
            Enemy enemy = nearbyObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemyFound = true;
                break;
            }
        }

        if (!enemyFound)
        {
            GameControl.control.spawnPoint.position = potentialSpawnPosition;
        }
        searching = false;
    }

    void FixedUpdate()
    {
        if (!searching)
        {
            Vector2 currentPosition = transform.position;
            StartCoroutine(FindNewSpawnPoint(currentPosition));
        }
    }
}
