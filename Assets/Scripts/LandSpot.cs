using UnityEngine;

public class LandSpot : MonoBehaviour
{
    private Vector3 boxSize;

    private void Start()
    {
        // Set the box size based on the land spot dimensions
        boxSize = new Vector3(1, 0.16f, 1);
    }

    public void MoveLandSpot(Vector3 targetPosition)
    {
        Vector3 validPosition = GetValidLandPosition(targetPosition);
        transform.position = Vector3.Lerp(transform.position, validPosition, Time.deltaTime * 7);
    }

    private Vector3 GetValidLandPosition(Vector3 desiredPosition)
    {
        RaycastHit hit;
        Vector3 adjustedPosition = desiredPosition;

        // BoxCast to detect collisions around the target position
        if (Physics.BoxCast(desiredPosition, boxSize / 2, Vector3.down, out hit, Quaternion.identity, Mathf.Infinity))
        {
            if (hit.collider != null && hit.collider.gameObject != gameObject)
            {
                // Adjust position to not intersect with the wall
                adjustedPosition = hit.point;
                adjustedPosition.y = transform.position.y; // Keep the y position consistent
            }
        }

        return adjustedPosition;
    }
}
