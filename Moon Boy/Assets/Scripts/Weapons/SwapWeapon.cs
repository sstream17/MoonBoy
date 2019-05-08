using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapWeapon : MonoBehaviour {
    public Image swapImage;
    public Animator animator;
    public GameObject playerObject;
    public PlayerMovement player;
    public PrefabWeapon prefabWeapon;


    void Start() {
        animator = swapImage.GetComponent<Animator>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<PlayerMovement>();
        prefabWeapon = playerObject.GetComponentInChildren<PrefabWeapon>();
    }

    public void Swap() {
        animator.Play("Swap", 0, 0);
        prefabWeapon.weapon = player.weaponSpawn.weapon;
        prefabWeapon.SetTimer();
    }


    public void AttemptSwap() {
        bool playerOnCollectible = player.playerOnCollectible;
        if (playerOnCollectible) {
            Swap();
        }
    }
}
