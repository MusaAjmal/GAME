using UnityEngine;

public class SlingShot : MonoBehaviour
{
    public GameObject stonePrefab;
    private Vector3 initialMousePosition;
    public float sensitivity = 100.0f;
    private GameObject currentStone;
    public float stoneHeight = 10.0f;
    [SerializeField] public float angle = 75f, gravity = 20f;
    Vector3 currentVelocity;
    private CharacterController characterController;
    float horizontalDistance;
    Vector3 direction;
    private bool stoneInFlight = false;
    private Camera mainCamera;

    private void Awake()
    {
        // Initialize main camera reference
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Inventory.Instance.defaultItem != null)
        {
            stonePrefab = Inventory.Instance.defaultItem.prefab.gameObject;
        }

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        HandleMouseInput(); // Use mouse input for editor, standalone, or web builds
#endif

#if UNITY_ANDROID || UNITY_IOS || UNITY_WSA
        HandleTouchInput(); // Use touch input for Android, iOS, or Windows Store Apps
#endif

        if (currentStone != null && stoneInFlight)
        {
            currentVelocity.y -= gravity * Time.deltaTime;
            characterController.Move(currentVelocity * Time.deltaTime);

            // Check if the stone has landed
            if (characterController.isGrounded)
            {
                LandStone();
            }
        }
    }

    private void HandleMouseInput()
    {
      /*  // Detect mouse click
        if (Input.GetMouseButtonDown(0))
        {
            initialMousePosition = Input.mousePosition;
            CheckForDistraction(); // Check for distractions when mouse is clicked
        }

        if (Input.GetMouseButtonUp(0) && !stoneInFlight)
        {
            LaunchStone(); // Launch the stone if it is not already in flight
        }*/
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    initialMousePosition = touch.position;
                    CheckForDistraction(); // Check for distractions on touch began
                    break;

                case TouchPhase.Ended:
                    // Launch the stone if it is not already in flight
                    if (!stoneInFlight)
                    {
                        LaunchStone();
                    }
                    break;
            }
        }
    }

    private void CheckForDistraction()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null && hit.collider.CompareTag("Distraction"))
            {
                // Set the direction to the distraction's position
                direction = hit.collider.transform.position - transform.position;
                direction.y = 0; // Keep the direction flat
            }
            else
            {
                direction = Vector3.zero; // Reset direction if not a distraction
            }
        }
    }

    private void LaunchStone()
    {
        if (stonePrefab != null && direction != Vector3.zero) // Check if direction is valid
        {
            currentStone = Instantiate(stonePrefab, transform.position + Vector3.up * stoneHeight, Quaternion.identity);
            characterController = currentStone.GetComponent<CharacterController>();
            shoot();

            stonePrefab = null;
            Inventory.Instance.RemoveItem(Inventory.Instance.defaultItem);
            stoneInFlight = true;
        }
        else
        {
            Debug.Log("No ITEM in inventory left to YEET or clicked on a non-distraction");
        }
    }

    void shoot()
    {
        horizontalDistance = direction.magnitude; // Set the horizontal distance to the distance to the distraction
        direction.Normalize(); // Normalize the direction

        Debug.Log("direction: " + direction);
        Debug.Log("horizontalDistance: " + horizontalDistance);

        float speed = Mathf.Sqrt(horizontalDistance * gravity / Mathf.Sin(2 * angle * Mathf.Deg2Rad));
        float verticalSpeed = Mathf.Sin(angle * Mathf.Deg2Rad) * speed;
        float horizontalSpeed = Mathf.Cos(angle * Mathf.Deg2Rad) * speed;

        currentVelocity = new Vector3(horizontalSpeed * direction.x, verticalSpeed, horizontalSpeed * direction.z);
    }

    void LandStone()
    {
        stoneInFlight = false;
        currentVelocity = Vector3.zero;

        if (currentStone != null && currentStone.tag != "Bone")
        {
            Invoke("DestroyStone", 0.1f);
        }
    }

    void DestroyStone()
    {
        Destroy(currentStone);
    }
}
