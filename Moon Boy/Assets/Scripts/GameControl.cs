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

    public Transform currentPlayer;
    public Transform playerPrefab;
    public Transform spawnPoint;
    public Joystick joystick;

    private PrefabWeapon prefabWeapon;
    private SwapWeapon swapWeapon;
    private ToggleWeapon toggleWeapon;

    public TextMeshProUGUI healthDisplay;
    public TextMeshProUGUI energyDisplay;
    public TextMeshProUGUI ammoDisplay;
    public TextMeshProUGUI grenadeDisplay;

    public Image swapImage;
    public Image grenadeImage;
    public Image ammoImage;


    void SetUI() {
        control.healthDisplay.text = control.currentPlayer.GetComponent<Player>().health.ToString("0");
        control.energyDisplay.text = control.currentPlayer.GetComponent<PlayerMovement>().energy.ToString("0");
        control.ammoDisplay.text = control.playerWeapon.ammo.ToString("0");
        control.grenadeDisplay.text = control.prefabWeapon.grenades.ToString("0");
    }


    void InitializePlayer() {
        control.currentPlayer = Instantiate(control.playerPrefab, control.spawnPoint.position, Quaternion.identity);
        control.prefabWeapon = control.currentPlayer.GetComponentInChildren<PrefabWeapon>();
        control.swapWeapon = control.currentPlayer.GetComponent<SwapWeapon>();
        control.toggleWeapon = control.currentPlayer.GetComponent<ToggleWeapon>();
        control.camera.GetComponent<CinemachineVirtualCamera>().Follow = control.currentPlayer;
        SetUI();
    }


    void Awake() {
        if (control == null) {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this) {
            Destroy(gameObject);
        }
        control.spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        InitializePlayer();
    }


    void RespawnPlayer() {
        InitializePlayer();
    }


    public void KillPlayer(Player playerToKill) {
		GameObject deathEffectClone = Instantiate(playerToKill.deathEffect, playerToKill.transform.position, Quaternion.identity);
		Destroy(playerToKill.gameObject);
        RespawnPlayer();
		Destroy(deathEffectClone, 5);
	}


    public void Shoot() {
        control.prefabWeapon.SendShoot();
    }


    public void Swap() {
        control.swapWeapon.AttemptSwap();
    }


    public void Toggle() {
        control.toggleWeapon.Toggle();
    }
}
