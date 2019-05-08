using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapWeapon : MonoBehaviour {
    public Image swapImage;

    public void Swap() {
        Animator animator = swapImage.GetComponent<Animator>();
        animator.Play("Swap", 0, 0);
    }
}
