using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour {

    public Animator animator;

	public void FadeToNextLevel () {
		FadeToLevel((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
	}


	public void FadeToLevel (int levelIndex) {
		GameControl.control.levelToLoad = levelIndex;
		animator.SetTrigger("FadeOut");
	}


	public void OnFadeComplete () {
		SceneManager.LoadScene(GameControl.control.levelToLoad);
	}


    void OnTriggerEnter2D (Collider2D hitInfo) {
		if (hitInfo.name == "Player") {
			FadeToNextLevel();
		}
	}
}
