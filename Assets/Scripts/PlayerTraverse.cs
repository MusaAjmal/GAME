using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerTraverse : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;

    }
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
    public LayerMask wallLayer; // Layer mask for walls
    public GameObject[] distractions;
    public LayerMask distractionLayer; // Layer for distractions

    private void Start()
    {
        player = Player.Instance; // Get the singleton instance of the player
        distractions = GameObject.FindGameObjectsWithTag("Distraction");
    }

    void Update()
    {

        // Check for mouse input using the legacy input system
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        HandleMouseInput(); // Use mouse input for editor, standalone, or web builds
#endif

#if UNITY_ANDROID || UNITY_IOS || UNITY_WSA
        HandleTouchInput(); // Use touch input for Android, iOS, or Windows Store Apps
#endif

        // Move the player towards the target position
        if (isMoving)
        {
            MovePlayer();

        }
    }
    public void disablemove()
    {
        isMoving = false;
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0)) // 0 corresponds to the left mouse button
        {
            // Check if the click was on a UI element or if the slingshot is active
            if (IsPointerOverUIElement() || CheckDistractionClick()/* || ToggleButton.isSlingshotActive*/)
            {
                Debug.Log("Clicked on a UI element or slingshot is active, not moving the player.");
                return; // Do nothing if the click was on a UI element or slingshot is active
            }

            Vector3 mousePosition = Input.mousePosition;

            // Convert the screen position to world position, keeping the Y fixed
            Vector3 worldPosition = ScreenToWorldPositionOrthographic(mousePosition);
            worldPosition.y = fixedYPosition; // Ensure Y position remains fixed

            // Find the closest point to the clicked position
            GameObject closestPoint = GetClosestPoint(worldPosition);

            // If a point was found and the path is clear, set it as the target position
            if (closestPoint != null && IsPathClear(player.transform.position, closestPoint.transform.position))
            {

                targetPosition = closestPoint.transform.position;
                isMoving = true;
            }
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0) // Check if there are any touches
        {
            Touch touch = Input.GetTouch(0); // Get the first touch

            // Check if the touch is at the start phase
            if (touch.phase == UnityEngine.TouchPhase.Began)
            {
                // Check if the touch was on a UI element or if the slingshot is active
                if (IsPointerOverUIElement(touch.fingerId) || CheckDistractionTouch())
                {
                    Debug.Log("Touched on a UI element or slingshot is active, not moving the player.");
                    return; // Do nothing if the touch was on a UI element or slingshot is active
                }

                Debug.Log("The distraction click is: " + CheckDistractionTouch());

                Vector3 touchPosition = touch.position;

                // Convert the screen position to world position, keeping the Y fixed
                Vector3 worldPosition = ScreenToWorldPositionOrthographic(touchPosition);
                worldPosition.y = fixedYPosition; // Ensure Y position remains fixed

                // Find the closest point to the touched position
                GameObject closestPoint = GetClosestPoint(worldPosition);

                // If a point was found and the path is clear, set it as the target position
                if (closestPoint != null && IsPathClear(player.transform.position, closestPoint.transform.position))
                {
                    targetPosition = closestPoint.transform.position;
                    isMoving = true;
                }
                else
                {

                }
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
        // Adjust the Z coordinate based on the camera's perspective for correct depth
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
            Debug.Log("Path is blocked by a wall.");
            // Path is blocked
            SoundPlayer.PlayOneShotSound("wrong");
            return false;
        }

        // Path is clear
        return true;
    }

    // Improved UI Detection Method
    private bool IsPointerOverUIElement(int fingerId = -1)
    {
        // Create a new PointerEventData to simulate touch
        PointerEventData eventData = new PointerEventData(EventSystem.current);

        if (fingerId >= 0)
        {
            eventData.position = Input.GetTouch(fingerId).position;
        }
        else
        {
            eventData.position = Input.mousePosition;
        }

        // List to store Raycast results
        List<RaycastResult> results = new List<RaycastResult>();

        // Iterate over all canvases and perform raycast
        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            // Get the GraphicRaycaster attached to the current canvas
            GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();

            // Check if the canvas and raycaster exist
            if (canvas == null || raycaster == null)
                continue;

            // Perform the raycast
            raycaster.Raycast(eventData, results);

            // If any results are found, a UI element was touched
            if (results.Count > 0)
                return true;
        }

        // Return false if no UI element was found
        return false;
    }
    public void OnPlayerRespawn()
    {
        //targetPosition = respawnPosition; // Reset the target position to the respawn position
        isMoving = false; // Ensure that the player is not moving after respawn
    }

    // Method to move the player towards the target
    private void MovePlayer()
    {
        /*        SoundPlayer.PlayOneShotSound("walk");
        */
        float step = moveSpeed * Time.deltaTime;

        // Calculate the next position while keeping the Y position fixed
        Vector3 nextPosition = Vector3.MoveTowards(player.transform.position, targetPosition, step);
        nextPosition.y = fixedYPosition; // Ensure Y position remains fixed

        // Calculate the direction to the target position, keeping Y as zero
        Vector3 targetDirection = (targetPosition - player.transform.position);
        targetDirection.y = 0; // Ensure the Y component is zero to maintain the fixed height

        // Calculate the rotation needed to look at the target direction
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // Smoothly rotate the player towards the target direction
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, 10 * Time.deltaTime);

        // Move the player to the next position
        player.transform.position = nextPosition;

        // Check if the player has reached the target position
        if (Vector3.Distance(player.transform.position, targetPosition) < 0.001f)
        {
            isMoving = false;
        }
    }


    private bool CheckDistractionClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform a raycast that only hits objects in the distraction layer
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, distractionLayer))
        {
            // Check if the hit object is within the distractions array
            foreach (GameObject distraction in distractions)
            {
                Debug.Log("Distraction: " + distraction.name);
                if (distraction != null && hit.collider.gameObject == distraction)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckDistractionTouch()
    {
        // Ensure there's at least one touch input
        if (Input.touchCount > 0)
        {
            // Get the first touch
            Touch touch = Input.GetTouch(0);

            // Convert the touch position to a ray
            Ray ray = mainCamera.ScreenPointToRay(touch.position);
            RaycastHit hit;

            // Perform a raycast that only hits objects in the distraction layer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, distractionLayer))
            {
                // Check if the hit object is within the distractions array
                foreach (GameObject distraction in distractions)
                {
                    Debug.Log("Distraction: " + distraction.name);
                    if (distraction != null && hit.collider.gameObject == distraction)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

}
