using UnityEngine;

public class TorchMovement : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 rightHandOffset = new Vector3(0.5f, 0f, 0f); // Offset when facing right
    private PlayerMovement playerMovement;
    private Vector3 leftHandOffset;

    void Start()
    {
        // Get player movement script
        if (playerTransform == null)
        {
            playerTransform = transform.parent;
        }
        
        if (playerTransform != null)
        {
            playerMovement = playerTransform.GetComponent<PlayerMovement>();
        }

        // Calculate left hand offset (mirror of right hand)
        leftHandOffset = new Vector3(-rightHandOffset.x, rightHandOffset.y, rightHandOffset.z);
    }

    void Update()
    {
        if (playerMovement != null)
        {
            float inputX = Input.GetAxisRaw("Horizontal");

            if (inputX < -0.01f) // Moving left
            {
                transform.localPosition = rightHandOffset;
                transform.localRotation = Quaternion.Euler(0, -180, -41);
            }
            else if (inputX > 0.01f) // Moving right
            {
                transform.localPosition = leftHandOffset;
                transform.localRotation = Quaternion.Euler(0, -180, 41);
            }
            else // Not moving
            {
                // Keep position based on facing direction
                if (playerMovement.IsFacingRight())
                {
                    transform.localPosition = rightHandOffset;
                }
                else
                {
                    transform.localPosition = rightHandOffset;
                }
                // Reset to default rotation
                transform.localRotation = Quaternion.Euler(0, -180, -41);
            }
        }
    }
}
