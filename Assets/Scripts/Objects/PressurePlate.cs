using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private GameObject connectedDoor;
    [SerializeField] private Animator plateAnimator;
    [SerializeField] private Door doorScript;
    [SerializeField] private Door1 door1Script;
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private AudioSource doorSound;
    [SerializeField] private BoxCollider2D boxCollider;

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
                doorSound.Play();
            }

            if (doorScript != null)
            {
                doorScript.isOpen = true;
            }

            else
            {
                Debug.LogWarning("PressurePlate: Door script is not assigned!");
            }

            if (door1Script != null)
            {
                door1Script.Open = true;
            }
            
            if (plateAnimator != null)
            {
                plateAnimator.SetBool("pressed", true);
            }
            if (SceneManager.GetActiveScene().buildIndex == 5)
            {
                boxCollider.isTrigger = true;
            }
        }
    }
}