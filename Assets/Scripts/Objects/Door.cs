using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private Collider2D col;
    public bool isOpen = false;
    [SerializeField] private GameObject winningScene;
    void Start()
    {
        col = GetComponent<Collider2D>();

        if (col != null && !col.isTrigger)
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isOpen)
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            if (currentSceneIndex == 1)
            {
                SceneManager.LoadScene(2);
            }
            else if (currentSceneIndex == 2)
            {
                SceneManager.LoadScene(3);
            }
            else if (currentSceneIndex == 3)
            {
                SceneManager.LoadScene(4);
            }
            else if (currentSceneIndex == 4)
            {
                SceneManager.LoadScene(5);
            }
            else if (currentSceneIndex == 5)
            {
                winningScene.SetActive(true);
            }
        }
        else if (collision.CompareTag("Player") && !isOpen)
        {
            Debug.Log("Player tried to enter door but it's not open yet.");
        }
    }
}