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
    private bool canMove = true; // Flag to control movement

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Input1.OnDash += Input_OnDash;
        rb = GetComponent<Rigidbody>();
    }

    private void Input_OnDash(object sender, System.EventArgs e)
    {
        if (canDash && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

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
            StartCoroutine(MovePlayer());
        }
    }

    private IEnumerator MovePlayer()
    {
        Vector3 MovementVector;
        Vector2 inputVector = Input1.Move();
        
       
            MovementVector = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

       
        
        if (transform.position.y != 1.2f)
        {
            Vector3 temp = new Vector3(transform.position.x, 1.2f, transform.position.z);
            transform.position = temp;
        }
        else
        {
            if (MovementVector.magnitude >= 0.1f)
            {
                TurnUsingBuiltInMethod(MovementVector);
                characterController.Move(MovementVector * moveSpeed * Time.deltaTime);
            }
        }

        yield return null;
    }

    private void TurnUsingBuiltInMethod(Vector3 MovementVector)
    {
        transform.forward = Vector3.Slerp(transform.forward, MovementVector, rotateSpeed * Time.deltaTime);
    }

    private void TurnUsingMaths(Vector3 directionVector)
    {
        if (directionVector.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(directionVector.x, directionVector.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float turnAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotateSpeed, turnSmoothTime);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            transform.rotation = Quaternion.Euler(0, turnAngle, 0);
            characterController.Move(moveDir * moveSpeed * Time.deltaTime);
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
