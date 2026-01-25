using System;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Collider2D col;
    private Animator anim;
    public bool isOpen = false;
    [SerializeField] private GameObject WinningScene;

    void Start()
    {
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isOpen)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 1)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(2);
            }
            else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 2)
            {
                WinningScene.SetActive(true);
            }
        }
    }
}