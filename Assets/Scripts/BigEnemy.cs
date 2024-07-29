using System.Collections;
using UnityEngine;

public class BigEnemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Vector3 faceDirection;
    private float rotateSpeed = 5f;
    private float lookDistance = 100f;
    private Vector3 lastInteractDirection;

    [SerializeField] public float noiseRadius;
    [SerializeField] private LayerMask objectMask;
    [SerializeField] private LayerMask obstructionMask;
    [SerializeField] private LayerMask distractionMask;
    [SerializeField] private float collisionRange = 10f;
    public bool noiseDetected = false;
    [SerializeField] private float noiseAttentionTime = 2f;
    private GameObject throwableObject;

    [SerializeField] private float torchCheckRange = 10f;

    private enum EnemyState
    {
        Idle,
        Alerted
    }
    private EnemyState currentState;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        currentState = EnemyState.Idle;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        if (currentState == EnemyState.Idle)
        {
            CheckTorch();
        }
    }

    private bool IsMoving()
    {
        return faceDirection != Vector3.zero;
    }

    public IEnumerator MoveToNoise(Vector3 noisePosition, GameObject throwableObject)
    {
        currentState = EnemyState.Alerted;

        while (Vector3.Distance(transform.position, noisePosition) > 2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, noisePosition, moveSpeed * Time.deltaTime);
            faceDirection = (noisePosition - transform.position).normalized;
            transform.forward = Vector3.Slerp(transform.forward, faceDirection, rotateSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(noiseAttentionTime);

        while (Vector3.Distance(transform.position, initialPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
            faceDirection = (initialPosition - transform.position).normalized;
            transform.forward = Vector3.Slerp(transform.forward, faceDirection, rotateSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = initialRotation;

        currentState = EnemyState.Idle;
    }

    public void CheckDistraction(Vector3 noisePosition, GameObject throwableObject)
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, noiseRadius, distractionMask);

        if (rangeChecks.Length != 0)
        {
            GameObject targetObject = null;
            Vector3 targetPosition = Vector3.zero;

            foreach (Collider collider in rangeChecks)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Distraction"))
                {
                    Vector3 directionToTarget = collider.transform.position - transform.position;
                    float distanceToTarget = directionToTarget.magnitude;
                    directionToTarget.Normalize();

                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    {
                        targetObject = collider.gameObject;
                        targetPosition = collider.transform.position;
                        Debug.Log("Target object found: " + targetObject.name); // Debug log for target object found
                        break;
                    }
                }
            }

            if (targetObject != null)
            {
                StopAllCoroutines();
                StartCoroutine(MoveToNoise(targetPosition, targetObject));
            }
            else
            {
                Debug.Log("No target object found"); // Debug log for no target object found
            }
        }
        else
        {
            Debug.Log("No distractions in range"); // Debug log for no distractions in range
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected"); // Debug log for collision detected
    }

    public static bool ObjectIsInLayerMask(GameObject obj, LayerMask mask) => (mask.value & (1 << obj.layer)) != 0;

    public void CheckTorch()
    {
        GameObject torch = GameObject.FindGameObjectWithTag("Torch");

        if (torch == null)
        {
            Debug.Log("Torch not found");
            return;
        }

        Torch torchComponent = torch.GetComponent<Torch>();

        if (torchComponent == null)
        {
            Debug.Log("Torch component not found");
            return;
        }

        if (torchComponent.IsToggledOn)
        {
            Debug.Log("Torch is already on");
            return;
        }

        Vector3 torchPosition = torch.transform.position;

        if (Vector3.Distance(transform.position, torchPosition) > torchCheckRange)
        {
            Debug.Log("Torch is out of range");
            return;
        }

        StartCoroutine(MoveToTorch(torchPosition, torchComponent));
    }

    private IEnumerator MoveToTorch(Vector3 torchPosition, Torch torchComponent)
    {
        currentState = EnemyState.Alerted;

        while (Vector3.Distance(transform.position, torchPosition) > 2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, torchPosition, moveSpeed * Time.deltaTime);
            faceDirection = (torchPosition - transform.position).normalized;
            transform.forward = Vector3.Slerp(transform.forward, faceDirection, rotateSpeed * Time.deltaTime);
            yield return null;
        }

        // Toggle the torch off
        torchComponent.Toggle(true);

        yield return new WaitForSeconds(noiseAttentionTime);

        while (Vector3.Distance(transform.position, initialPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
            faceDirection = (initialPosition - transform.position).normalized;
            transform.forward = Vector3.Slerp(transform.forward, faceDirection, rotateSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = initialRotation;

        currentState = EnemyState.Idle;
    }
}
