using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class GameControl : MonoBehaviour
{
    public static GameControl control;

    public GameObject camera;

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
        if (control.camera == null) {
            GameObject searchResult = GameObject.FindGameObjectWithTag("Camera");
            if (searchResult != null) {
                control.camera = searchResult;
            }
            else {
                return;
            }
        }
        control.spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        Debug.Log(control.enemyWeapons[0].ToString() + control.enemyWeapons[1].ToString() + control.enemyWeapons[2].ToString());
    }


    void RespawnPlayer(Player playerToSpawn) {
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
