using System;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private GameObject connectedDoor;
    [SerializeField] private Animator anim;
    [SerializeField] private Animator animator;
    [SerializeField] private Door doorScript;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") || col.CompareTag("Block"))
        {
            anim.SetBool("open", true);
            doorScript.isOpen = true;
            animator.SetBool("pressed", true);
        }
    }
}