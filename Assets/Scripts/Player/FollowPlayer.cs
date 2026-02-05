using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float maxX;
    [SerializeField] private float minX;

    void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = playerTransform.position.x;
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        transform.position = newPosition;
    }
}
