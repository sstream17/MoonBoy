using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static GameControl control;

    public int levelToLoad;

    public int current_palette_index = 0;

    public Weapon playerWeapon;

    public int playerAmmo;

    public int playerLives = 3;

    public Transform spawnPoint;

    public Weapon[] enemyWeapons;

    public LevelTransition levelTransition;

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
        control.levelTransition = GameObject.FindGameObjectWithTag("LevelTransition").GetComponent<LevelTransition>();
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
        yield break;
    }

    private void GameOver()
    {
        GameObject UI = GameObject.Find("UI");
        GameOver gameOver = UI.GetComponent<GameOver>();
        gameOver.DisplayGameOver();
    }


    public void KillPlayer(Player playerToKill) {
        playerLives = playerLives - 1;
        playerToKill.gameObject.SetActive(false);
		GameObject deathEffectClone = Instantiate(playerToKill.deathEffect, playerToKill.transform.position, Quaternion.identity);
        if (playerLives <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(RespawnPlayer(playerToKill));
        }

		Destroy(deathEffectClone, 5);
	}

}
