using System;
using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public LevelTransition LevelTransition;

    public GameObject ShootArea;
    public GameObject SwapButton;
    public GameObject ToggleButton;

    public GameObject JoystickTrigger;
    public GameObject ShootTrigger;
    public GameObject SwapTrigger;
    public GameObject ToggleTrigger;

    public GameObject Cursor;
    public Animator CursorAnimator;
    public Vector2 CursorPosition_Joystick;
    public Vector2 CursorPosition_Shoot;
    public Vector2 CursorPosition_Swap;
    public Vector2 CursorPosition_Toggle;

    public Weapon[] enemyWeapons;

    public GameObject[] Cameras;

    public enum Area { Shoot = 0, Swap = 1, Toggle = 2 };

    private int currentStage = 0;

    private Camera mainCamera;

    void Awake()
    {
        GameControl.control.enemyWeapons = enemyWeapons;
    }


    IEnumerator WaitToStartAnimation(float time, Action methodToCall)
    {
        yield return new WaitForSecondsRealtime(time);
        methodToCall();
    }

    public IEnumerator WaitToStartAnimation(float cameraHeight, Area area, Action<Area> methodToCall)
    {
        while (mainCamera.transform.position.y > cameraHeight + 0.01f)
        {
            yield return new WaitForEndOfFrame();
        }

        methodToCall(area);
    }

    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(WaitToStartAnimation(2f, StartJoystickAnimation));
    }

    private void StartJoystickAnimation()
    {
        JoystickTrigger.SetActive(true);
        Cursor.transform.localPosition = CursorPosition_Joystick;
        Cursor.SetActive(true);
        CursorAnimator.SetTrigger("Joystick");
    }

    public void StartTapAnimation(Area area)
    {
        switch (area)
        {
            case Area.Shoot:
                Cursor.transform.localPosition = CursorPosition_Shoot;
                break;

            case Area.Swap:
                SwapTrigger.SetActive(true);
                SwapButton.SetActive(true);
                Cursor.transform.localPosition = CursorPosition_Swap;
                break;

            case Area.Toggle:
                Cursor.transform.localPosition = CursorPosition_Toggle;
                break;
        }

        Cursor.SetActive(true);
        CursorAnimator.SetTrigger("Tap");
    }

    public void StopAnimation()
    {
        CursorAnimator.SetTrigger("Hide");
    }

    public void AdvanceTutorial(GameObject trigger)
    {
        if (trigger != null)
        {
            trigger.SetActive(false);
        }

        StopAnimation();
        currentStage = currentStage + 1;

        switch (currentStage)
        {
            case 1:
                ShootTrigger.SetActive(true);
                ShootArea.SetActive(true);
                break;
            case 2:
                ActivateCamera(0);
                break;
            case 3:
                ActivateCamera(0);
                break;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            playerMovement.enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Player player = hitInfo.GetComponent<Player>();
        if (player != null)
        {
            PlayerPrefs.SetInt(PlayerPrefConstants.Tutorial, -1);
            LevelTransition.FadeToLevel(0);
        }
    }

    public void ActivateCamera(int index)
    {
        for (int i = 0; i < Cameras.Length; i++)
        {
            if (i == index)
            {
                Cameras[i].SetActive(true);
            }
            else
            {
                Cameras[i].SetActive(false);
            }
        }
    }
}
