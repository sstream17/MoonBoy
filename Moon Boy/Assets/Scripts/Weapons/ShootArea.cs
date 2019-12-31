using UnityEngine;
using UnityEngine.EventSystems;

public class ShootArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PrefabWeapon PrefabWeapon;
    public bool buttonPressed;
    public Vector2 pointerPosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        pointerPosition = eventData.pressPosition;
        PrefabWeapon.SendShoot();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }
}