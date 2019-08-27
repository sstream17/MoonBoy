using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandUIObject : MonoBehaviour
{
    public enum TutorialObject { Joystick = 0, Swap = 1, Toggle = 2 };

    public TutorialObject ThisTutorialObject;

    public Animator Animator;

    private void OnEnable()
    {
        switch (ThisTutorialObject)
        {
            case TutorialObject.Joystick:
                Animator.Play("ExpandJoystick");
                break;

            case TutorialObject.Swap:
                Animator.Play("ExpandSwapButton");
                break;

            case TutorialObject.Toggle:
                Animator.Play("ExpandToggleButton");
                break;
        }
    }
}
