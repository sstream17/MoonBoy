using UnityEngine;
using UnityEngine.EventSystems;

public class ShootArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler
{
    public PrefabWeapon PrefabWeapon;
    public bool buttonPressed;
    public Vector2 pointerPosition;

    private Vector2 staticPointerPosition;
    private Vector2 draggingPointerPosition;
    private bool dragging = false;

    private void SetPointerPosition()
    {
        if (dragging)
        {
            pointerPosition = draggingPointerPosition;
        }
        else
        {
            pointerPosition = staticPointerPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        staticPointerPosition = eventData.position;
        SetPointerPosition();
        PrefabWeapon.SendShoot();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragging = true;
        draggingPointerPosition = eventData.position;
        SetPointerPosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
    }
}
