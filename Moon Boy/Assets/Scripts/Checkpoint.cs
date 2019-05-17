using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    void OnTriggerEnter2D (Collider2D hitInfo) {
		Player player = hitInfo.GetComponent<Player>();
		if (player != null) {
            Vector2 newPosition = player.transform.position;
            newPosition.y = newPosition.y + 1f;
			GameControl.control.spawnPoint.position = newPosition;
		}
	}
}
