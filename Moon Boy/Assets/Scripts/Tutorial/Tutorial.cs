using System;
using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public LevelTransition LevelTransition;

    public GameObject Joystick;
    public GameObject ShootArea;
    public GameObject SwapButton;
    public GameObject ToggleButton;

    public GameObject JoystickTrigger;
    public GameObject ShootTrigger;
    public GameObject SwapTrigger;
    public GameObject ToggleTrigger;
    public GameObject GrenadeTrigger;

    public GameObject Cursor;
    public Animator CursorAnimator;
    public Vector2 CursorPosition_Joystick;
    public Vector2 CursorPosition_Shoot;
    public Vector2 CursorPosition_Swap;
    public Vector2 CursorPosition_Toggle;
    public Vector2 CursorPosition_Grenade;

    public PrefabWeapon PlayerWeapon;

    public Weapon[] enemyWeapons;

    public GameObject[] Cameras;

    public enum Area { Shoot = 0, Swap = 1, Toggle = 2, Grenade = 3 };

    public bool WaitingForGrenade = false;

    private int currentStage = 0;

    private Camera mainCamera;

    private bool mustToggleToGrenade = false;
    private bool mustThrowGrenade = false;

    void Awake()
    {
        GameControl.control.enemyWeapons = enemyWeapons;
    }


    IEnumerator WaitToStartJoystickAnimation(float time, Action methodToCall)
    {
        yield return new WaitForSecondsRealtime(time);
        methodToCall();
        EnablePlayerMovement();
    }

    public IEnumerator WaitToStartAnimation(Vector2 cameraPosition, Area area, Action<Area> methodToCall)
    {
        float distance = (cameraPosition - (Vector2) mainCamera.transform.position).magnitude;
        while (distance > 0.05f)
        {
            distance = (cameraPosition - (Vector2) mainCamera.transform.position).magnitude;
            yield return new WaitForEndOfFrame();
        }

        methodToCall(area);
    }

    private void EnablePlayerMovement()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            playerMovement.enabled = true;
        }
    }

    private void DisablePlayerMovement()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            playerMovement.animator.SetFloat("Speed", 0f);
            playerMovement.animator.SetFloat("VerticalSpeed", 0f);
            playerMovement.enabled = false;
        }
    }

    void Start()
    {
        DisablePlayerMovement();
        mainCamera = Camera.main;
        StartCoroutine(WaitToStartJoystickAnimation(2f, StartJoystickAnimation));
    }

    private void Update()
    {
        if (WaitingForGrenade)
        {
            if (!PlayerWeapon.isGrenade)
            {
                if (!mustToggleToGrenade)
                {
                    if (currentStage == 4)
                    {
                        currentStage = 3;
                    }

                    StartTapAnimation(Area.Toggle);
                }
                mustToggleToGrenade = true;
                mustThrowGrenade = false;
                GrenadeTrigger.SetActive(false);
                ShootArea.SetActive(false);
            }
            else
            {
                if (!mustThrowGrenade)
                {
                    StartTapAnimation(Area.Grenade);
                }
                mustToggleToGrenade = false;
                mustThrowGrenade = true;
                GrenadeTrigger.SetActive(true);
                ShootArea.SetActive(true);
            }
        }


    }

    private void StartJoystickAnimation()
    {
        Joystick.SetActive(true);
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
                ToggleTrigger.SetActive(true);
                ToggleButton.SetActive(true);
                Cursor.transform.localPosition = CursorPosition_Toggle;
                break;

            case Area.Grenade:
                Cursor.transform.localPosition = CursorPosition_Grenade;
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

            case 4:
                return;

            case 5:
                WaitingForGrenade = false;
                ActivateCamera(0);
                break;
        }

        EnablePlayerMovement();
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
