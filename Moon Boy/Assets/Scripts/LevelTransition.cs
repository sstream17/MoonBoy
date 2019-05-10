using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour {

    public Animator animator;

	private int levelToLoad;


	public void FadeToNextLevel () {
		FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
	}


	public void FadeToLevel (int levelIndex) {
		levelToLoad = levelIndex;
		animator.SetTrigger("FadeOut");
	}


	public void OnFadeComplete () {
		SceneManager.LoadScene(levelToLoad);
	}


    void OnTriggerEnter2D (Collider2D hitInfo) {
		PlayerMovement player = hitInfo.GetComponent<PlayerMovement>();
		if (player != null) {
			FadeToNextLevel();
		}
	}
}
