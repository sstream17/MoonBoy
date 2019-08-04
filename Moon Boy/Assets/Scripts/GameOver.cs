using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject controlsUI;
    public GameObject pauseUI;
    public GameObject gameOverUI;
    public Animator animator;

    public void DisplayGameOver()
    {
        controlsUI.SetActive(false);
        pauseUI.SetActive(false);
        gameOverUI.SetActive(true);
    }


    public void Exit()
    {
        GameControl.control.levelTransition.FadeToLevel(0);
    }
}
