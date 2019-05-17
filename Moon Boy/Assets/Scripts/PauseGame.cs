using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject controlsUI;
    public GameObject pauseUI;


    public void Pause() {
        controlsUI.SetActive(false);
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
    }


    public void Resume() {
        pauseUI.SetActive(false);
        controlsUI.SetActive(true);
        Time.timeScale = 1f;
    }


    public void Exit() {
        GameControl.control.levelTransition.FadeToLevel(0);
    }
}
