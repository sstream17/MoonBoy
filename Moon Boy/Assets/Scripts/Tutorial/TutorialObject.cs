using UnityEngine;

public class TutorialObject : MonoBehaviour
{
    public Tutorial Tutorial;
    public GameObject Trigger;

    private RectTransform rectTransform;
    private Vector3[] corners;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
    }

    private void Update()
    {
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
