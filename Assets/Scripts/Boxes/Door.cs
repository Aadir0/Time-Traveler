using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private Collider2D col;
    private Animator anim;
    public bool isOpen = false;
    [SerializeField] private GameObject winningSceneCanvas;

    void Start()
    {
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        if (col != null && !col.isTrigger)
        {
            Debug.LogWarning("Door collider is not set as trigger! Setting it now.");
            col.isTrigger = true;
        }

        if (winningSceneCanvas != null)
        {
            winningSceneCanvas.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isOpen)
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            if (currentSceneIndex == 1)
            {
                Debug.Log("Player entered door in scene 1. Loading scene 2.");
                SceneManager.LoadScene(2);
            }
            else if (currentSceneIndex == 2)
            {
                if (winningSceneCanvas != null)
                {
                    Debug.Log("Player entered door in scene 2. Enabling winning scene canvas.");
                    winningSceneCanvas.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("Winning scene canvas is not assigned!");
                }
            }
        }
        else if (collision.CompareTag("Player") && !isOpen)
        {
            Debug.Log("Player tried to enter door but it's not open yet.");
        }
    }
}