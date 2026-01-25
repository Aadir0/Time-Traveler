using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private TimeTracker timeTracker;

    [System.Obsolete]
    void Start()
    {
        // Find the TimeTracker in the scene
        timeTracker = FindObjectOfType<TimeTracker>();
        
        if (timeTracker == null)
        {
            Debug.LogError("TimeTracker not found in scene!");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            timeTracker.SetCheckpoint(transform.position);
        }
    }
}
