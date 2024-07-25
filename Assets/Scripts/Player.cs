using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private Input1 Input1;
    [SerializeField] private Rigidbody rb;
    [SerializeField] public float pickupDistance;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Joystick joystick;

    [SerializeField] private Transform cam;
    private float dashDistance = 5f;
    private float dashDuration = 0.375f;
    private float turnSmoothTime = 0.1f;
    private bool isDashing = false;
    private bool canDash = true;
    private float dashCooldown = 3f;
    static public bool canMove = true; // Flag to control movement
    GameObject SlingShot;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SlingShot = GameObject.FindGameObjectWithTag("SlingShot");
        //Input1.OnDash += Input_OnDash;
        rb = GetComponent<Rigidbody>();
    }

   /* private void Input_OnDash(object sender, System.EventArgs e)
    {
        if (canDash && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }*/

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        canMove = false; // Disable movement during dash

        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + transform.forward * dashDistance;

        float elapsedTime = 0f;
        while (elapsedTime < dashDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / dashDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
        isDashing = false;
        canMove = true; // Re-enable movement after dash

        // Start cooldown timer
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void Update()
    {
        if (canMove)
        {
            MovePlayer();
        }
        if (IsMoving())
        {
            SlingShot.SetActive(false);
        }
        else
        {
            SlingShot.SetActive(true);
        }
    }

    private void MovePlayer()
    {
        Vector3 MovementVector = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

        if (MovementVector.magnitude >= 0.1f)
        {
            // Convert input to isometric movement
            MovementVector = ConvertToIsometric(MovementVector);

            // Calculate movement distance and character height
            float moveDistance = moveSpeed * Time.deltaTime;
            float playerRadius = 0.7f;
            float playerHeight = 1.156f;

            // Check if movement is possible
            bool canMove = CanMove(MovementVector, moveDistance, playerRadius, playerHeight);

            if (canMove)
            {
                // Execute movement
                characterController.Move(MovementVector * moveSpeed * Time.deltaTime);

                // Adjust player rotation
                TurnUsingBuiltInMethod(MovementVector);

                // Keep player at a fixed y-level
                MaintainGroundLevel();
            }
        }
    }

    private bool CanMove(Vector3 movementVector, float moveDistance, float playerRadius, float playerHeight)
    {
        // Check for obstacles using CapsuleCast
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, movementVector, moveDistance);

        if (!canMove)
        {
            // Attempt movement only along the x-axis
            Vector3 moveX = new Vector3(movementVector.x, 0, 0).normalized;
            canMove = movementVector.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveX, moveDistance);

            if (canMove)
            {
                movementVector = moveX;
            }
            else
            {
                // Attempt movement only along the z-axis
                Vector3 moveZ = new Vector3(0, 0, movementVector.z).normalized;
                canMove = movementVector.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveZ, moveDistance);

                if (canMove)
                {
                    movementVector = moveZ;
                }
            }
        }

        return canMove;
    }

    private Vector3 ConvertToIsometric(Vector3 input)
    {
        // Rotate the input vector to match isometric perspective
        Quaternion isometricRotation = Quaternion.Euler(0, 45f, 0);
        return isometricRotation * input;
    }

    private void TurnUsingBuiltInMethod(Vector3 movementVector)
    {
        // Smoothly rotate the player to face the movement direction
        transform.forward = Vector3.Slerp(transform.forward, movementVector, rotateSpeed * Time.deltaTime);
    }

    private void MaintainGroundLevel()
    {
        // Ensure player stays on the correct ground level
        if (Mathf.Abs(transform.position.y - 1.076f) > 0.01f)
        {
            Vector3 temp = new Vector3(transform.position.x, 1.076f, transform.position.z);
            transform.position = temp;
        }
    }

    public bool IsMoving()
    {
        Vector3 movementVector = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        return movementVector.magnitude >= 0.1f;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
