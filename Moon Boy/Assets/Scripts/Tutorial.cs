using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public LevelTransition LevelTransition;

    public GameObject Cursor;

    public Animator CursorAnimator;

    public Vector2 CursorPosition_SwipeJoystick;


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
        Cursor.transform.localPosition = CursorPosition_SwipeJoystick;
        Cursor.SetActive(true);
        CursorAnimator.SetTrigger("Joystick");
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
