﻿using System;
using System.Collections;
using System.Collections.Generic;
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

    public Animator ArrowAnimator;

    public PrefabWeapon PlayerWeapon;

    public Weapon[] enemyWeapons;

    public GameObject[] Cameras;

    public enum Area { Shoot = 0, Swap = 1, Toggle = 2, Grenade = 3 };

    public bool WaitingForGrenade = false;

    private int currentStage = 0;

    private Camera mainCamera;

    private bool mustToggleToGrenade = false;
    private bool mustThrowGrenade = false;
    private bool firstToggle = true;

    private RectTransform rectTransform;

    private Dictionary<string, float> shootAreaInitialValues = new Dictionary<string, float>();

    private Dictionary<string, float> shootTriggerValues = new Dictionary<string, float>
    {
        { "width", 300f }, 
        { "height", 200f }, 
        { "x", 500f }, 
        { "y", 20f }, 
    };

    private Dictionary<string, float> grenadeTriggerValues = new Dictionary<string, float>
    {
        { "width", 200f },
        { "height", 200f },
        { "x", 265f },
        { "y", 310f },
    };

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

    public IEnumerator WaitToStartAnimation(Vector2 cameraPosition, Area area, float time, Action<Area, float> methodToCall)
    {
        float distance = (cameraPosition - (Vector2) mainCamera.transform.position).magnitude;
        while (distance > 0.05f)
        {
            distance = (cameraPosition - (Vector2) mainCamera.transform.position).magnitude;
            yield return new WaitForEndOfFrame();
        }

        if (area.Equals(Area.Toggle))
        {
            WaitingForGrenade = true;
        }

        methodToCall(area, time);
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
        rectTransform = ShootArea.GetComponent<RectTransform>();
        shootAreaInitialValues = new Dictionary<string, float>
        {
            { "width", rectTransform.rect.width },
            { "height", rectTransform.rect.height },
            { "x", rectTransform.localPosition.x },
            { "y", rectTransform.localPosition.y },
        };

        DisablePlayerMovement();
        mainCamera = Camera.main;
        StartCoroutine(WaitToStartJoystickAnimation(2f, StartJoystickAnimation));
    }

    private void Update()
    {
        if (WaitingForGrenade)
        {
            float time = firstToggle ? 1f : 0f;
            firstToggle = false;
            if (!PlayerWeapon.isGrenade)
            {
                if (!mustToggleToGrenade)
                {
                    if (currentStage == 4)
                    {
                        currentStage = 3;
                    }

                    StartTapAnimation(Area.Toggle, time);
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
                    StartTapAnimation(Area.Grenade, 0f);
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
        StartCoroutine(DelayCursorAnimation(1f, "Joystick"));
    }

    IEnumerator DelayCursorAnimation(float time, string animation)
    {
        yield return new WaitForSecondsRealtime(time);
        Cursor.SetActive(true);
        CursorAnimator.SetTrigger(animation);
    }

    public void StartTapAnimation(Area area, float time)
    {
        switch (area)
        {
            case Area.Shoot:
                ShootTrigger.SetActive(true);
                ShootArea.SetActive(true);
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

        StartCoroutine(DelayCursorAnimation(time, "Tap"));
    }

    public void StopAnimation()
    {
        CursorAnimator.SetTrigger("Hide");
    }

    IEnumerator WaitForGrenadeToExplode()
    {
        yield return new WaitForSeconds(5f);
        SetShootAreaPosition(shootAreaInitialValues);
        ActivateCamera(0);
        ArrowAnimator.SetTrigger("Continue");
        EnablePlayerMovement();
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
                SetShootAreaPosition(shootTriggerValues);
                break;

            case 2:
                ActivateCamera(0);
                break;

            case 3:
                SetShootAreaPosition(grenadeTriggerValues);
                ActivateCamera(0);
                break;

            case 4:
                return;

            case 5:
                WaitingForGrenade = false;
                StartCoroutine(WaitForGrenadeToExplode());
                return;
        }

        ArrowAnimator.SetTrigger("Continue");
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

    private void SetShootAreaPosition(Dictionary<string, float> values)
    {
        rectTransform.sizeDelta = new Vector2
        {
            x = values["width"],
            y = values["height"],
        };

        rectTransform.localPosition = new Vector2
        {
            x = values["x"],
            y = values["y"]
        };
    }
}
