using UnityEngine;
using System.Collections;

public class Pushable : MonoBehaviour
{
    private static bool anyBoxMoving = false; // Tracks if any box is currently being pushed
    
    private Rigidbody2D rb;
    private bool isMoving = false;
    
    [SerializeField] private float pushDuration = 0.5f; // Slower movement (increased from 0.2f)
    [SerializeField] private float gridSize = 1f; // One unit movement per push

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.mass = 1f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isMoving && !anyBoxMoving)
        {
            Debug.Log("Player collided with box");
            
            if (collision.rigidbody != null)
            {
                // Direction from player to box
                Vector2 dir = (rb.position - collision.rigidbody.position).normalized;
                
                // Determine the dominant direction (snap to cardinal directions)
                Vector2 pushDirection = Vector2.zero;
                if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                {
                    pushDirection = new Vector2(Mathf.Sign(dir.x) * gridSize, 0f);
                }
                else
                {
                    pushDirection = new Vector2(0f, Mathf.Sign(dir.y) * gridSize);
                }

                Debug.Log("Push direction: " + pushDirection);
                
                // Check if there's a block in the target position
                if (!IsBlockedInDirection(pushDirection))
                {
                    Debug.Log("Moving box");
                    StartCoroutine(MoveBlock(pushDirection));
                }
                else
                {
                    Debug.Log("Box is blocked");
                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isMoving && !anyBoxMoving)
        {
            if (collision.rigidbody != null && collision.rigidbody.linearVelocity.magnitude > 0.1f)
            {
                // Direction from player to box
                Vector2 dirToBox = (rb.position - collision.rigidbody.position).normalized;
                
                // Player's movement direction
                Vector2 playerMoveDir = collision.rigidbody.linearVelocity.normalized;
                
                // Check if player is moving TOWARDS the box (not away)
                float dotProduct = Vector2.Dot(playerMoveDir, dirToBox);
                
                if (dotProduct > 0.3f) // Player is moving towards the box
                {
                    // Snap to cardinal direction based on direction from player to box
                    Vector2 pushDirection = Vector2.zero;
                    if (Mathf.Abs(dirToBox.x) > Mathf.Abs(dirToBox.y))
                    {
                        pushDirection = new Vector2(Mathf.Sign(dirToBox.x) * gridSize, 0f);
                    }
                    else
                    {
                        pushDirection = new Vector2(0f, Mathf.Sign(dirToBox.y) * gridSize);
                    }

                    // Check if there's a block in the target position
                    if (!IsBlockedInDirection(pushDirection))
                    {
                        StartCoroutine(MoveBlock(pushDirection));
                    }
                }
            }
        }
    }

    private bool IsBlockedInDirection(Vector2 direction)
    {
        Vector2 targetPosition = rb.position + direction;
        
        // Check for any colliders at the target position (checking for one grid unit ahead)
        Collider2D hit = Physics2D.OverlapCircle(targetPosition, 0.3f);
        
        // If there's a collider at the target position (not this object and not the player)
        if (hit != null && hit.gameObject != gameObject && !hit.CompareTag("Player"))
        {
            return true; // Blocked
        }
        
        return false; // Not blocked
    }

    private IEnumerator MoveBlock(Vector2 direction)
    {
        isMoving = true;
        anyBoxMoving = true; // Lock all boxes from being pushed
        
        Vector2 startPosition = rb.position;
        Vector2 targetPosition = startPosition + direction; // Only moves exactly one grid unit
        float elapsedTime = 0f;

        // Freeze all velocities to prevent flying
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Smooth movement over the push duration
        while (elapsedTime < pushDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / pushDuration);
            rb.MovePosition(Vector2.Lerp(startPosition, targetPosition, t));
            yield return null;
        }

        // Ensure final position is exact (snap to grid)
        rb.MovePosition(targetPosition);
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        
        isMoving = false;
        anyBoxMoving = false; // Unlock boxes for next push
    }
}