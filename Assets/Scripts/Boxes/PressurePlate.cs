using System;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private GameObject connectedDoor;
    [SerializeField] private Animator plateAnimator;
    [SerializeField] private Door doorScript;
    [SerializeField] private Animator doorAnimator;

    private void Start()
    {
        if (connectedDoor != null && doorAnimator == null)
        {
            doorAnimator = connectedDoor.GetComponent<Animator>();
        }

        if (connectedDoor != null && doorScript == null)
        {
            doorScript = connectedDoor.GetComponent<Door>();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") || col.CompareTag("Block"))
        {
            if (doorAnimator != null)
            {
                doorAnimator.SetBool("open", true);
            }

            if (doorScript != null)
            {
                doorScript.isOpen = true;
            }
            else
            {
                Debug.LogWarning("PressurePlate: Door script is not assigned!");
            }

            if (plateAnimator != null)
            {
                plateAnimator.SetBool("pressed", true);
            }
        }
    }
}