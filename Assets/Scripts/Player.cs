using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Versioning;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private Input1 Input1;
    [SerializeField] public float pickupDistance;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private CharacterController characterController;
    
    [SerializeField] private Transform cam;
    private float dashDistance = 5f;
    private float dashDuration = 0.2f;
    private float turnSmoothTime = 0.1f;
    private bool isDashing = false;
  


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
        Input1.OnDash += Input_OnDash;
    }

    private void Input_OnDash(object sender, System.EventArgs e)
    {
        if (!isDashing)
        {
            StartCoroutine(Dash());
        }
    }
    private IEnumerator Dash()
    {
        isDashing = true;
        
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + transform.forward * dashDistance;

        float elapsedTime = 0;
        while (elapsedTime < dashDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / dashDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
        isDashing = false;
    }

    private void Update()
    {
        

        MovePlayer();
        
    }
    private void MovePlayer()
    {
        Vector2 inputVector = Input1.Move();
        Vector3 MovementVector = new Vector3(-(inputVector.x),0,-inputVector.y);
        
        if(MovementVector.magnitude >= 0.1f)
        {
            //TurnUsingMaths(MovementVector);
            
            TurnUsingBuiltInMethod(MovementVector);
            characterController.Move(MovementVector * moveSpeed * Time.deltaTime);

            
        }
       // transform.position += MovementVector * moveSpeed * Time.deltaTime;
        
    }
    private void TurnUsingBuiltInMethod(Vector3 MovementVector)
    {
        transform.forward = Vector3.Slerp(transform.forward, MovementVector, rotateSpeed * Time.deltaTime);
    }
    private void TurnUsingMaths(Vector3 directionVector)
    {
        if(directionVector.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(directionVector.x, directionVector.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float turnAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotateSpeed, turnSmoothTime);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            transform.rotation = Quaternion.Euler (0,turnAngle, 0);
            characterController.Move(moveDir * moveSpeed * Time.deltaTime);

        }
    }
    public Vector3 GetPosition()
    {
        return transform.position;
    }
   
}
