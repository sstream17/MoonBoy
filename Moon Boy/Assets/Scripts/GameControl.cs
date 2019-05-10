using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static GameControl control;

    public int levelToLoad;

    public int current_palette_index = 0;

    public Weapon playerWeapon;

    public Transform spawnPoint;

    public int[] enemyWeapons;


    void Awake() {
        if (control == null) {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this) {
            Destroy(gameObject);
        }
        control.spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
    }


    void RespawnPlayer(Player playerToSpawn) {
        playerToSpawn.gameObject.GetComponent<PlayerMovement>().energy = 100f;
        playerToSpawn.health = 100;
        playerToSpawn.UpdateUI();
        playerToSpawn.gameObject.transform.position = control.spawnPoint.position;
        playerToSpawn.gameObject.SetActive(true);
    }


    public void KillPlayer(Player playerToKill) {
        playerToKill.gameObject.SetActive(false);
		GameObject deathEffectClone = Instantiate(playerToKill.deathEffect, playerToKill.transform.position, Quaternion.identity);
        RespawnPlayer(playerToKill);
		Destroy(deathEffectClone, 5);
	}

}
