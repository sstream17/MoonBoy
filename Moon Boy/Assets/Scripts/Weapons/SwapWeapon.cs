using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapWeapon : MonoBehaviour {
    public Image swapImage;
    public Animator animator;


    void Start() {
        animator = swapImage.GetComponent<Animator>();
    }

    public void Swap() {
        animator.Play("Swap", 0, 0);
    }
}
