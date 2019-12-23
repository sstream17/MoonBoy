using UnityEngine;
using UnityEngine.UI;

public class ButtonMask : MonoBehaviour
{
    void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }
}
