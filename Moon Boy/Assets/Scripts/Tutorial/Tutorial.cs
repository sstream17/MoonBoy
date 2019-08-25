using Cinemachine;
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

    public GameObject cameraOne;
    public GameObject cameraTwo;

    public enum Area { Shoot = 0, Swap = 1, Toggle = 2 };

    void Awake()
    {
        GameControl.control.enemyWeapons = enemyWeapons;
    }


    IEnumerator WaitToStartAnimation()
    {
        yield return new WaitForSecondsRealtime(2f);
        StartJoystickAnimation();
    }

    void Start()
    {
        StartCoroutine(WaitToStartAnimation());
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
                ShootTrigger.SetActive(true);
                ShootArea.SetActive(true);
                Cursor.transform.localPosition = CursorPosition_Shoot;
                ActivateCameraTwo();
                break;

            case Area.Swap:
                SwapTrigger.SetActive(true);
                SwapButton.SetActive(true);
                Cursor.transform.localPosition = CursorPosition_Swap;
                break;

            case Area.Toggle:
                ToggleTrigger.SetActive(true);
                ToggleButton.SetActive(true);
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

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Player player = hitInfo.GetComponent<Player>();
        if (player != null)
        {
            PlayerPrefs.SetInt(PlayerPrefConstants.Tutorial, -1);
            LevelTransition.FadeToLevel(0);
        }
    }

    public void ActivateCameraTwo()
    {
        cameraTwo.SetActive(true);
        cameraOne.SetActive(false);
    }

    public void ActivateCameraOne()
    {
        cameraOne.SetActive(true);
        cameraTwo.SetActive(false);
    }
}
