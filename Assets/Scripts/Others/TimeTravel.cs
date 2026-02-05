using UnityEngine;

public class TimeTravel : MonoBehaviour
{
    [SerializeField] private float teleportInterval = 3f;
    [SerializeField] private float checkpointSpacing = 8f; // Distance between auto checkpoints

    private float timer;
    private Vector3 lastCheckpoint;
    private Vector3 lastFramePosition;
    private float distanceTravelled = 0f;

    public Transform player;

    void Start()
    {
        timer = teleportInterval;
        lastCheckpoint = player.position;
        lastFramePosition = player.position;
    }

    void Update()
    {
        HandleAutoCheckpoint();
        HandleTeleportTimer();
    }

    void HandleAutoCheckpoint()
    {
        // Calculate distance moved since last frame
        float distanceThisFrame = Vector3.Distance(player.position, lastFramePosition);
        distanceTravelled += distanceThisFrame;
        lastFramePosition = player.position;

        // Check if accumulated distance travelled exceeds checkpoint spacing
        if (distanceTravelled >= checkpointSpacing)
        {
            lastCheckpoint = player.position;
            distanceTravelled = 0f; // Reset accumulated distance
            timer = teleportInterval; // Reset teleport timer for the new checkpoint
        }
    }

    void HandleTeleportTimer()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            TeleportToCheckpoint();
            timer = teleportInterval;
        }
    }

    void TeleportToCheckpoint()
    {
        player.position = lastCheckpoint;
    }
}