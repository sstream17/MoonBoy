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
    public Vector2 CursorPosition_SwipeJoystick;
    public Vector2 CursorPosition_Shoot;
    public Vector2 CursorPosition_Swap;
    public Vector2 CursorPosition_Toggle;

    public Weapon[] enemyWeapons;

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
        Cursor.transform.localPosition = CursorPosition_SwipeJoystick;
        Cursor.SetActive(true);
        CursorAnimator.SetTrigger("Joystick");
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
}
