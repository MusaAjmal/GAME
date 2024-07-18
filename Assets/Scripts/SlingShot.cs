using System.Collections.Generic;
using UnityEngine;

public class SlingShot : MonoBehaviour
{
    public GameObject stonePrefab;
   [SerializeField] public GameObject landPosition;
    private Vector3 initialMousePosition;
    private Vector3 finalMousePosition;
    private Vector3 currentVelocity;
    [Range(0f, 1000f)]
    public float sensitivity;
    private GameObject currentStone;
    public float stoneHeight = 10.0f;
    [SerializeField] public float angle = 75f, gravity = 20f;

   // private Vector3 initialMousePosition;
    //private Vector3 finalMousePosition;
    //private GameObject currentStone;
    private CharacterController characterController;
    float horizontalDistance;
    float speed;
    Vector3 direction;

    private void Awake()
    {
        // Any initialization if needed
    }

    void Update()
    {
        if (Inventory.Instance.defaultItem != null)
        {
            stonePrefab = Inventory.Instance.defaultItem.prefab.gameObject;
        }
        HandleMouseInput();
        UpdateCurrentStonePosition();
    }
    private void Start()
    {
        
        
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetInitialMousePosition();
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentMousePosition = Input.mousePosition;
            direction = initialMousePosition - currentMousePosition;
            direction.z = direction.y;
            direction.y = 0;

            horizontalDistance = direction.magnitude * 1;
            Vector3 targetPosition = new Vector3(-direction.x, 0, -direction.z);
            landPosition.transform.position = Vector3.Lerp(landPosition.transform.position, targetPosition, Time.deltaTime * 10);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (stonePrefab != null) {

                currentStone = Instantiate(stonePrefab, transform.position + Vector3.up * (stoneHeight), Quaternion.identity);
                characterController = currentStone.GetComponent<CharacterController>();
                finalMousePosition = Input.mousePosition;

                direction = initialMousePosition - finalMousePosition;
                direction.z = direction.y;
                direction.y = 0;

                Vector3 targetPosition = new Vector3(-direction.x, 0, -direction.z);
                landPosition.transform.position = Vector3.Lerp(landPosition.transform.position, targetPosition, Time.deltaTime * 10);
                shoot();
                stonePrefab = null; 
                Inventory.Instance.RemoveItem(Inventory.Instance.defaultItem);

            }
            else
            {
                Debug.Log("No Item Left To YEET");
            }
            
        }
    }

    private void SetInitialMousePosition()
    {
        initialMousePosition = Input.mousePosition;
        Debug.Log("initialMousePosition: " + initialMousePosition);
    }

    private void UpdateLandPosition()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        direction = initialMousePosition - currentMousePosition;
        direction.z = direction.y;
        direction.y = 0;

        horizontalDistance = direction.magnitude * 1;
        Vector3 targetPosition = new Vector3(-direction.x, 0, -direction.z);
        landPosition.transform.position = Vector3.Lerp(landPosition.transform.position, targetPosition, Time.deltaTime * 10);
    }

    private void LaunchStone()
    {
        if (stonePrefab != null)
        {
            currentStone = Instantiate(stonePrefab, transform.position + Vector3.up * stoneHeight, Quaternion.identity);
            characterController = currentStone.GetComponent<CharacterController>();
            finalMousePosition = Input.mousePosition;

            direction = initialMousePosition - finalMousePosition;
            direction.z = direction.y;
            direction.y = 0;

            Vector3 targetPosition = new Vector3(-direction.x, 0, -direction.z);
            landPosition.transform.position = Vector3.Lerp(landPosition.transform.position, targetPosition, Time.deltaTime * 10);
        }
        else
        {
            Debug.Log("No Item Left to Throw");
        }
       
       // CalculateAndSetVelocity();
    }

    private void UpdateCurrentStonePosition()
    {
        if (currentStone != null)
        {
            currentVelocity.y -= gravity * Time.deltaTime;
            characterController.Move(currentVelocity * Time.deltaTime);
        }
    }

    void shoot()
    {
        horizontalDistance = direction.magnitude * 1;
        Vector3 targetPosition = new Vector3(-direction.x, 0, -direction.z);
        landPosition.transform.position = Vector3.Lerp(landPosition.transform.position, targetPosition, Time.deltaTime * 10);
        direction = new Vector3(-direction.x, 0, -direction.z).normalized;

        Debug.Log("direction: " + direction);
        Debug.Log("horizontalDistance: " + horizontalDistance);

        speed = horizontalDistance * gravity;
        speed /= Mathf.Sin(2 * angle * (Mathf.PI / 180f));
        speed = Mathf.Sqrt(speed);

        float verticalSpeed = Mathf.Sin(angle * (Mathf.PI / 180f)) * speed;
        float horizontalSpeed = Mathf.Cos(angle * (Mathf.PI / 180f)) * speed;

        currentVelocity = new Vector3(horizontalSpeed * direction.x, verticalSpeed, horizontalSpeed * direction.z);
    }
}