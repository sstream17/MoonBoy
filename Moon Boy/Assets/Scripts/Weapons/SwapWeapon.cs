using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapWeapon : MonoBehaviour {
    public Animator animator;
    public GameObject playerObject;
    public PlayerMovement player;
    public PrefabWeapon prefabWeapon;


    void Start() {
        animator = GameControl.control.swapImage.GetComponent<Animator>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<PlayerMovement>();
    }

    public void Swap() {
        animator.Play("Swap", 0, 0);
        GameControl.control.playerWeapon = player.weaponSpawn.weapon;
        PrefabWeapon.SetTimer();
    }


    public void AttemptSwap() {
        bool playerOnCollectible = player.playerOnCollectible;
        if (playerOnCollectible) {
            Swap();
        }
    }
}
