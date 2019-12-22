using System.Linq;
using UnityEngine;

public class TutorialObject : MonoBehaviour
{
    public Tutorial Tutorial;
    public GameObject Trigger;

    private RectTransform rectTransform;
    private Vector3[] corners;
    private bool updatingCorners = false;
    private int numberOfSameIterations = 3;
    private int currentCountSameIterations = 0;

    private Vector3[] WaitForCorners(Vector3[] corners)
    {
        Vector3[] initialCorners = corners;
        Vector3[] newCorners = new Vector3[4];
        rectTransform.GetWorldCorners(newCorners);
        if (initialCorners.SequenceEqual(newCorners))
        {
            currentCountSameIterations = currentCountSameIterations + 1;
        }

        if (currentCountSameIterations >= numberOfSameIterations)
        {
            updatingCorners = false;
        }

        return newCorners;
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        updatingCorners = true;
    }

    private void Update()
    {
        if (updatingCorners)
        {
            corners = WaitForCorners(corners);
        }

        var touches = Input.touchCount;
        for (int i = 0; i < touches; i++)
        {
            var touch = Input.GetTouch(i);
            bool objectTouched = TouchIsInBounds(touch.position, corners[0], corners[2]);
            if (objectTouched)
            {
                Tutorial.AdvanceTutorial(Trigger);
                gameObject.SetActive(false);
                break;
            }
        }
    }

    private bool TouchIsInBounds(Vector2 touch, Vector2 minBound, Vector2 maxBound)
    {
        if (touch.x < minBound.x || touch.x > maxBound.x)
        {
            return false;
        }

        if (touch.y < minBound.y || touch.y > maxBound.y)
        {
            return false;
        }

        return true;
    }
}
