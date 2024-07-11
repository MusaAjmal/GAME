using System.Collections.Generic;
using UnityEngine;

public class SlingShot : MonoBehaviour
{
    // List to hold instantiated stones
    private List<GameObject> stones = new List<GameObject>();

    // Prefab for the stone
    public GameObject stonePrefab;

    // Variables to track mouse positions
    private Vector3 initialMousePosition;
    private Vector3 finalMousePosition;

    // Sensitivity factor for slingshot power
    public float sensitivity = 1000.0f;

    // Reference to the stone being launched
    private GameObject currentStone;

    // Fixed height for the stone
    public float stoneHeight = 10.0f;

    // Angle for the projectile launch
    [SerializeField] private float launchAngle = 45.0f;

    // Update is called once per frame
    void Update()
    {
        // Detect mouse click
        if (UnityEngine.Input.GetMouseButtonDown(0))
        {
            // Get the initial mouse position
            initialMousePosition = UnityEngine.Input.mousePosition;
            Debug.Log("initialMousePosition: " + initialMousePosition);

            // Instantiate a new stone at the slingshot's position
            currentStone = Instantiate(stonePrefab, transform.position + Vector3.up * stoneHeight, Quaternion.identity);
            stones.Add(currentStone);
        }

        // Detect mouse button release
        if (UnityEngine.Input.GetMouseButtonUp(0))
        {
            // Get the final mouse position
            finalMousePosition = UnityEngine.Input.mousePosition;

            // Calculate the direction and power of the shot
            Vector3 direction = initialMousePosition - finalMousePosition;
            direction.z = direction.y; // Swap y and z for 2D simulation in 3D space
            direction.y = 0; // Set y to zero for force calculations
            Vector3 force = direction * sensitivity;

            // Convert launch angle from degrees to radians
            float angleInRadians = launchAngle * Mathf.Deg2Rad;

            // Adjust force for angle
            float forceMagnitude = force.magnitude;
            Vector3 forceAtAngle = new Vector3(-force.x, Mathf.Sin(angleInRadians) * forceMagnitude, -force.z);

            // Apply force to the stone's Rigidbody
            Rigidbody rb = currentStone.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddForce(forceAtAngle, ForceMode.Impulse);
                Debug.Log("Force added: " + forceAtAngle);
            }

            // Clear the reference to the current stone
            currentStone = null;
        }

        // Update the position of the stone being dragged
        if (currentStone != null && !UnityEngine.Input.GetMouseButtonUp(0))
        {
            Vector3 currentMousePosition = UnityEngine.Input.mousePosition;
            Vector3 offset = (initialMousePosition - currentMousePosition) * sensitivity;
            currentStone.transform.position = new Vector3(transform.position.x + offset.x, stoneHeight, transform.position.z + offset.y);
        }
    }
}
