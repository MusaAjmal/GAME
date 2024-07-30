using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
public class PlayerTraverse : MonoBehaviour
{
    /* [SerializeField] private GameObject[] points; // Array of points to interact with
     private Player player; // Reference to the Player class

     [SerializeField] private float moveSpeed = 5f; // Speed of the player's movement
     private float constantYPosition = 1.076f; // Constant Y position for the player

     private bool isMoving = false; // Flag to check if the player is moving

     private void Start()
     {
         player = Player.Instance; // Get the Player instance
     }

     private void Update()
     {
         if (!isMoving)
         {
             HandleInput(); // Check for touch or mouse input
         }
     }

     private void HandleInput()
     {
         // Check for touch input
         if (Input.touchCount > 0)
         {
             Touch touch = Input.GetTouch(0);

             if (touch.phase == TouchPhase.Began)
             {
                 // Raycast to check if a point was touched
                 Ray ray = GetRayFromCamera(touch.position);
                 RaycastHit hit;

                 if (Physics.Raycast(ray, out hit))
                 {
                     HandleHit(hit);
                 }
             }
         }

         // Check for left mouse click
         if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
         {
             // Raycast to check if a point was clicked
             Ray ray = GetRayFromCamera(Input.mousePosition);
             RaycastHit hit;

             if (Physics.Raycast(ray, out hit))
             {
                 HandleHit(hit);
             }
         }
     }

     private Ray GetRayFromCamera(Vector3 screenPosition)
     {
         if (Camera.main.orthographic)
         {
             // Convert screen position to world position at the camera's near clip plane
             Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, Camera.main.nearClipPlane));
             // Orthographic cameras are aligned along the Z-axis, so direction is along the Z-axis
             Vector3 direction = Camera.main.transform.forward;

             return new Ray(worldPosition, direction);
         }
         else
         {
             // For non-orthographic cameras, use standard ScreenPointToRay
             return Camera.main.ScreenPointToRay(screenPosition);
         }
     }

     private void HandleHit(RaycastHit hit)
     {
         // Check if the hit object is one of the points
         foreach (GameObject point in points)
         {
             if (hit.collider.gameObject == point)
             {
                 Debug.Log(point.name);
                 // Set the target position to the touched or clicked point
                 Vector3 targetPosition = point.transform.position;
                 targetPosition.y = constantYPosition; // Ensure the y position is constant

                 // Start the movement coroutine
                 StartCoroutine(MoveToPoint(targetPosition));
                 break;
             }
         }
     }

     private IEnumerator MoveToPoint(Vector3 targetPosition)
     {
         isMoving = true; // Set the moving flag to true

         // Move the player towards the target position
         while (Vector3.Distance(player.transform.position, targetPosition) > 0.01f)
         {
             player.transform.position = Vector3.MoveTowards(
                 player.transform.position,
                 targetPosition,
                 moveSpeed * Time.deltaTime
             );

             yield return null; // Wait for the next frame
         }

         isMoving = false; // Set the moving flag to false
     }*/

    //////////////////////////GPT CODE///////////////////////////////

    /*public float moveSpeed = 5f; // Speed at which the player moves
    public GameObject[] points;  // Array of points the player can move towards

    private Vector3 targetPosition; // Current target position to move towards
    private bool isMoving = false;  // Flag to determine if the player is moving
    private Player player; // Reference to the Player singleton

    private const float fixedYPosition = 1.076f; // Fixed Y position for the player

    private void Start()
    {
        player = Player.Instance; // Get the singleton instance of the player
    }

    void Update()
    {
        // Check for mouse input using the legacy input system
        if (Input.GetMouseButtonDown(0)) // 0 corresponds to the left mouse button
        {
            // Get the mouse position in screen space
            Vector3 mousePosition = Input.mousePosition;

            // Convert the screen position to world position, keeping the Y fixed
            Vector3 worldPosition = ScreenToWorldPositionOrthographic(mousePosition);
            worldPosition.y = fixedYPosition; // Ensure Y position remains fixed

            // Find the closest point to the clicked position
            GameObject closestPoint = GetClosestPoint(worldPosition);

            // If a point was found, set it as the target position
            if (closestPoint != null)
            {
                targetPosition = closestPoint.transform.position;
                isMoving = true;
            }
        }

        // Move the player towards the target position
        if (isMoving)
        {
            float step = moveSpeed * Time.deltaTime;

            // Calculate the next position while keeping the Y position fixed
            Vector3 nextPosition = Vector3.MoveTowards(player.transform.position, targetPosition, step);
            nextPosition.y = fixedYPosition; // Ensure Y position remains fixed

            // Use the player instance to move the player
            player.transform.position = nextPosition;

            // Check if the player has reached the target position
            if (Vector3.Distance(player.transform.position, targetPosition) < 0.001f)
            {
                isMoving = false;
            }
        }
    }

    // Method to find the closest point from the array of points
    GameObject GetClosestPoint(Vector3 clickPosition)
    {
        GameObject closestPoint = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject point in points)
        {
            float distance = Vector3.Distance(clickPosition, point.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = point;
            }
        }

        return closestPoint;
    }

    // Method to convert screen position to world position for an orthographic camera with rotation
    Vector3 ScreenToWorldPositionOrthographic(Vector3 screenPosition)
    {
        // Adjust the Z coordinate based on camera's perspective for correct depth
        screenPosition.z = Camera.main.farClipPlane;

        // Get world point using raycast to ensure the position is on the correct plane
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.up * fixedYPosition);

        if (groundPlane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }

        // Fallback if the raycast fails (unlikely)
        return Vector3.zero;
    }*/

    /////////////
    ///
    public float moveSpeed = 5f; // Speed at which the player moves
    public GameObject[] points;  // Array of points the player can move towards

    private Vector3 targetPosition; // Current target position to move towards
    private bool isMoving = false;  // Flag to determine if the player is moving
    private Player player; // Reference to the Player singleton

    private const float fixedYPosition = 1.076f; // Fixed Y position for the player
    private LayerMask wallLayer; // Layer mask for walls

    private void Start()
    {
        player = Player.Instance; // Get the singleton instance of the player
        wallLayer = LayerMask.GetMask("Obstruction"); // Set the layer mask for walls
    }

    void Update()
    {
        // Check for mouse input using the legacy input system
        if (Input.GetMouseButtonDown(0)) // 0 corresponds to the left mouse button
        {
            // Get the mouse position in screen space
            Vector3 mousePosition = Input.mousePosition;

            // Convert the screen position to world position, keeping the Y fixed
            Vector3 worldPosition = ScreenToWorldPositionOrthographic(mousePosition);
            worldPosition.y = fixedYPosition; // Ensure Y position remains fixed

            // Find the closest point to the clicked position
            GameObject closestPoint = GetClosestPoint(worldPosition);

            // If a point was found and path is clear, set it as the target position
            if (closestPoint != null && IsPathClear(player.transform.position, closestPoint.transform.position))
            {
                targetPosition = closestPoint.transform.position;
                isMoving = true;
            }
        }

        // Move the player towards the target position
        if (isMoving)
        {
            float step = moveSpeed * Time.deltaTime;

            // Calculate the next position while keeping the Y position fixed
            Vector3 nextPosition = Vector3.MoveTowards(player.transform.position, targetPosition, step);
            nextPosition.y = fixedYPosition; // Ensure Y position remains fixed

            // Use the player instance to move the player
            player.transform.position = nextPosition;

            // Check if the player has reached the target position
            if (Vector3.Distance(player.transform.position, targetPosition) < 0.001f)
            {
                isMoving = false;
            }
        }
    }

    // Method to find the closest point from the array of points
    GameObject GetClosestPoint(Vector3 clickPosition)
    {
        GameObject closestPoint = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject point in points)
        {
            float distance = Vector3.Distance(clickPosition, point.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = point;
            }
        }

        return closestPoint;
    }

    // Method to convert screen position to world position for an orthographic camera with rotation
    Vector3 ScreenToWorldPositionOrthographic(Vector3 screenPosition)
    {
        // Adjust the Z coordinate based on camera's perspective for correct depth
        screenPosition.z = Camera.main.farClipPlane;

        // Get world point using raycast to ensure the position is on the correct plane
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.up * fixedYPosition);

        if (groundPlane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }

        // Fallback if the raycast fails (unlikely)
        return Vector3.zero;
    }

    // Method to check if the path is clear
    bool IsPathClear(Vector3 start, Vector3 end)
    {
        // Cast a ray between the start and end positions
        Ray ray = new Ray(start, (end - start).normalized);
        float distance = Vector3.Distance(start, end);

        // Check if the ray intersects with any colliders on the wall layer
        if (Physics.Raycast(ray, distance, wallLayer))
        {
            // Path is blocked
            return false;
        }

        // Path is clear
        return true;
    }
}
