using UnityEngine;
using UnityEngine.SceneManagement;

public class Door1 : MonoBehaviour
{
    private Collider2D col;
    public bool Open = false;
    void Start()
    {
        col = GetComponent<Collider2D>();

        if (col != null && !col.isTrigger)
        {
            Debug.LogWarning("Door1 collider is not set as trigger! Setting it now.");
            col.isTrigger = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Open)
        {
            SceneManager.LoadScene(4);
        }
    }
}
