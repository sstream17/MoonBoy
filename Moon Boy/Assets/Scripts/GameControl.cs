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

    private PrefabWeapon prefabWeapon;

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


    void RespawnPlayer() {
        return;
    }


    public void KillPlayer(Player playerToKill) {
		GameObject deathEffectClone = Instantiate(playerToKill.deathEffect, playerToKill.transform.position, Quaternion.identity);
		Destroy(playerToKill.gameObject);
        RespawnPlayer();
		Destroy(deathEffectClone, 5);
	}

}
