﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static GameControl control;

    public int levelToLoad;

    public int current_palette_index = 0;

    public Weapon playerWeapon;

    public int playerAmmo;

    public Transform spawnPoint;

    public Weapon[] enemyWeapons;

    public Animator transitionAnimator;


    void Awake() {
        if (control == null) {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this) {
            Destroy(gameObject);
        }
        control.spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        control.transitionAnimator = GameObject.FindGameObjectWithTag("LevelTransition").GetComponent<Animator>();
    }


    IEnumerator RespawnPlayer(Player playerToSpawn) {
        yield return new WaitForSeconds(0.5f);
        control.transitionAnimator.SetTrigger("Respawn");
        yield return new WaitForSeconds(1f);
        playerToSpawn.gameObject.GetComponent<PlayerMovement>().energy = 100f;
        playerToSpawn.health = 100;
        playerToSpawn.UpdateUI();
        playerToSpawn.gameObject.transform.position = control.spawnPoint.position;
        playerToSpawn.gameObject.SetActive(true);
        yield return false;
    }


    public void KillPlayer(Player playerToKill) {
        playerToKill.gameObject.SetActive(false);
		GameObject deathEffectClone = Instantiate(playerToKill.deathEffect, playerToKill.transform.position, Quaternion.identity);
        StartCoroutine(RespawnPlayer(playerToKill));
		Destroy(deathEffectClone, 5);
	}

}
