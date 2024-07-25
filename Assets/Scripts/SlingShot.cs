using UnityEngine;

public class SlingShot : MonoBehaviour
{
    public GameObject stonePrefab;
    [SerializeField] public GameObject landPosition;
    private Vector3 initialMousePosition;
    private Vector3 finalMousePosition;
    public float sensitivity = 1000.0f;
    private GameObject currentStone;
    public float stoneHeight = 10.0f;
    [SerializeField] public float angle = 75f, gravity = 20f;
    Vector3 currentVelocity;
    private CharacterController characterController;
    float horizontalDistance;
    float speed;
    Vector3 direction;
    private bool stoneInFlight = false;
    private Camera mainCamera;

    private void Awake()
    {
        // Initialize main camera reference
        mainCamera = Camera.main;
    }

    private void Start()
    {
        // Any other initialization code
    }

    void Update()
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
        // Detect mouse click
        if (Input.GetMouseButtonDown(0))
        {
            initialMousePosition = Input.mousePosition;
            Debug.Log("initialMousePosition: " + initialMousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentMousePosition = Input.mousePosition;
            direction = initialMousePosition - currentMousePosition;
            direction.z = direction.y;
            direction.y = 0;

            horizontalDistance = direction.magnitude * 0.1f; // Adjust this factor to control sensitivity
            Vector3 targetPosition = transform.position + new Vector3(direction.x, 0, direction.z).normalized * horizontalDistance;
            targetPosition.y = 0;

            // Smoothly move the land position towards the target position using Lerp
            Vector3 boxSize = new Vector3(1, 0.16f, 1); // Use provided dimensions
            landPosition.transform.position = Vector3.Lerp(landPosition.transform.position, GetValidLandPosition(targetPosition, boxSize), Time.deltaTime * 7);
        }

        if (Input.GetMouseButtonUp(0))
        {
            LaunchStone();
        }
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
                    Debug.Log("Touch Started: " + initialMousePosition);
                    break;

                case TouchPhase.Moved:
                    Vector3 currentTouchPosition = touch.position;
                    direction = initialMousePosition - currentTouchPosition;
                    direction.z = direction.y;
                    direction.y = 0;

                    horizontalDistance = direction.magnitude * 0.1f; // Adjust this factor to control sensitivity
                    Vector3 targetPosition = transform.position + new Vector3(direction.x, 0, direction.z).normalized * horizontalDistance;
                    targetPosition.y = 0;

                    // Move the land position directly to the target position
                    landPosition.transform.position = targetPosition;
                    break;

                case TouchPhase.Ended:
                    LaunchStone();
                    break;
            }
        }
    }

    private Vector3 GetValidLandPosition(Vector3 desiredPosition, Vector3 boxSize)
    {
        RaycastHit hit;
        Vector3 adjustedPosition = desiredPosition;

        // BoxCast to detect collisions around the target position
        if (Physics.BoxCast(desiredPosition, boxSize / 2, Vector3.up, out hit, Quaternion.identity, Mathf.Infinity))
        {
            Debug.Log("Hit Something! The object is: " + hit.collider.name);
            if (hit.collider != null && hit.collider.gameObject != landPosition)
            {
                // Adjust position to not intersect with the wall
                adjustedPosition = hit.point;
                adjustedPosition.y = landPosition.transform.position.y; // Keep the y position consistent
            }
        }

        return adjustedPosition;
    }

    private void LaunchStone()
    {
        if (stonePrefab != null)
        {
            currentStone = Instantiate(stonePrefab, transform.position + Vector3.up * stoneHeight, Quaternion.identity);
            characterController = currentStone.GetComponent<CharacterController>();
            finalMousePosition = Input.mousePosition; // For mouse fallback

            if (Input.touchCount > 0)
            {
                finalMousePosition = Input.GetTouch(0).position;
            }

            direction = initialMousePosition - finalMousePosition;
            direction.z = direction.y;
            direction.y = 0;

            shoot();
            stonePrefab = null;
            Inventory.Instance.RemoveItem(Inventory.Instance.defaultItem);
            stoneInFlight = true;
        }
        else
        {
            Debug.Log("No ITEM in inventory left to YEET");
        }
    }

    void shoot()
    {
        horizontalDistance = direction.magnitude * 0.1f; // Adjust this factor to control sensitivity
        Vector3 targetPosition = transform.position + new Vector3(direction.x, 0, direction.z).normalized * horizontalDistance;
        targetPosition.y = 0;
        direction = (targetPosition - transform.position).normalized;

        Debug.Log("direction: " + direction);
        Debug.Log("horizontalDistance: " + horizontalDistance);

        speed = Mathf.Sqrt(horizontalDistance * gravity / Mathf.Sin(2 * angle * Mathf.Deg2Rad));
        float verticalSpeed = Mathf.Sin(angle * Mathf.Deg2Rad) * speed;
        float horizontalSpeed = Mathf.Cos(angle * Mathf.Deg2Rad) * speed;

        currentVelocity = new Vector3(horizontalSpeed * direction.x, verticalSpeed, horizontalSpeed * direction.z);
    }

    void LandStone()
    {
        stoneInFlight = false;
        currentVelocity = Vector3.zero;

        if (currentStone.tag != "Bone")
        {
            Invoke("DestroyStone", 0.1f);
        }
    }

    void DestroyStone()
    {
        Destroy(currentStone);
    }
}
