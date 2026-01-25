using UnityEngine;

public class TimeTracker : MonoBehaviour
{
    [SerializeField] private float teleportInterval = 3f; // Time between automatic teleports
    
    private float timer;
    private Vector3 storedCheckpoint; // Stores the checkpoint position from collision
    private bool hasCheckpoint = false;

    public Transform player;

    void Start()
    {
        timer = teleportInterval;
        storedCheckpoint = player.position; // Default to starting position
    }

    void Update()
    {
        if (hasCheckpoint)
        {
            timer -= Time.deltaTime;

            // Teleport to checkpoint every 3 seconds
            if (timer <= 0f)
            {
                TeleportToCheckpoint();
                timer = teleportInterval;
            }
        }
    }

    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        storedCheckpoint = checkpointPosition;
        hasCheckpoint = true;
        timer = teleportInterval; // Reset timer when new checkpoint is set
        Debug.Log("Checkpoint set at: " + storedCheckpoint);
    }

    void TeleportToCheckpoint()
    {
        player.position = storedCheckpoint;
        Debug.Log("Teleported to checkpoint: " + storedCheckpoint);
    }
}
