using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

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

    private void Awake()
    {
        // Any initialization if needed
    }
    private void Start()
    {

    }

    void Update()
    {
        if (Inventory.Instance.defaultItem != null)
        {
            stonePrefab = Inventory.Instance.defaultItem.prefab.gameObject;
        }
        // Detect mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Get the initial mouse position
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
            landPosition.transform.position = Vector3.Lerp(landPosition.transform.position, targetPosition, Time.deltaTime * 7);
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Check if we have the item to throw
            if (stonePrefab != null)
            {
                currentStone = Instantiate(stonePrefab, transform.position + Vector3.up * stoneHeight, Quaternion.identity);
                characterController = currentStone.GetComponent<CharacterController>();
                finalMousePosition = Input.mousePosition;

                direction = initialMousePosition - finalMousePosition;
                direction.z = direction.y;
                direction.y = 0;

                shoot();
                stonePrefab = null;
                Inventory.Instance.RemoveItem(Inventory.Instance.defaultItem);

            }
            else
            {
                Debug.Log("No ITEM in inventory left to YEET");
            }
        }

        if (currentStone != null)
        {
            currentVelocity.y -= gravity * Time.deltaTime;
            characterController.Move(currentVelocity * Time.deltaTime);
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
}
