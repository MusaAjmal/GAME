using System.Collections;
using UnityEngine;

public class BigEnemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Vector3 faceDirection;
    private float rotateSpeed = 5f;
    private float lookDistance = 100f;
    private Vector3 lastInteractDirection;

    [SerializeField] public float noiseRadius = 6;
    [SerializeField] private LayerMask objectMask;
    [SerializeField] private LayerMask obstructionMask;
    [SerializeField] private LayerMask distractionMask;
    [SerializeField] private float collisionRange = 10f;
    public bool noiseDetected = false;
    [SerializeField] private float noiseAttentionTime = 2f;
    private GameObject throwableObject;

    [SerializeField] private float torchCheckRange = 10f;
    [SerializeField] public Items torch;

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
        CheckTorch();
        CheckPlayer();
    }

    private bool IsMoving()
    {
        return faceDirection != Vector3.zero;
    }

    public void CheckPlayer()
    {
        if (torch.isActive())
        {
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, noiseRadius);
            if (rangeChecks.Length != 0)
            {
                GameObject targetObject = null;
                Vector3 targetPosition = Vector3.zero;

                foreach (Collider collider in rangeChecks)
                {
                    if (collider.gameObject.tag == "Player")
                    {
                        Vector3 directionToTarget = (collider.transform.position - transform.position).normalized;
                        float distanceToTarget = Vector3.Distance(transform.position, collider.transform.position);

                        if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                        {
                            targetObject = collider.gameObject;
                            targetPosition = collider.transform.position;
                            break;
                        }
                        else
                        {
                            Debug.Log("Wall detected in front of the enemy while checking for player.");
                        }
                    }
                }

                if (targetObject != null)
                {
                    StopAllCoroutines();
                    StartCoroutine(MoveToPlayer(targetObject));
                }
            }
        }
    }

    public IEnumerator MoveToPlayer(GameObject playerObject)
    {
        currentState = EnemyState.Alerted;
        float playerDistance = Vector3.Distance(transform.position, playerObject.transform.position);

        if (playerDistance < noiseRadius)
        {
            if (!Physics.Raycast(transform.position, (playerObject.transform.position - transform.position).normalized, playerDistance, obstructionMask))
            {
                Vector3 noisePosition = playerObject.transform.position;

                while (Vector3.Distance(transform.position, noisePosition) > 2f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, noisePosition, moveSpeed * Time.deltaTime);
                    faceDirection = (noisePosition - transform.position).normalized;
                    transform.forward = Vector3.Slerp(transform.forward, faceDirection, rotateSpeed * Time.deltaTime);
                    yield return null;

                    // Check for wall during movement
                    if (Physics.Raycast(transform.position, (playerObject.transform.position - transform.position).normalized, out RaycastHit hit, playerDistance, obstructionMask))
                    {
                        Debug.Log("Wall detected in front of the enemy while moving to player.");
                        break;
                    }
                }

                if (Vector3.Distance(transform.position, noisePosition) < 2f)
                {
                    Debug.Log("Player caught");
                    // Game over logic here
                }
            }
            else
            {
                Debug.Log("Wall detected in front of the enemy while moving to player.");
            }
        }

        // Move back to the initial position
        yield return ReturnToInitialPosition();
    }

    private IEnumerator ReturnToInitialPosition()
    {
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

        // Move back to the initial position
        yield return ReturnToInitialPosition();
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
                        break;
                    }
                    else
                    {
                        Debug.Log("Wall detected in front of the enemy while checking for distractions.");
                    }
                }
            }

            if (targetObject != null)
            {
                StopAllCoroutines();
                StartCoroutine(MoveToNoise(targetPosition, targetObject));
            }
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

        Items torchComponent = torch.GetComponent<Items>();

        if (torchComponent == null)
        {
            Debug.Log("Torch component not found");
            return;
        }

        if (torchComponent.isActive())
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

    private IEnumerator MoveToTorch(Vector3 torchPosition, Items torchComponent)
    {
        currentState = EnemyState.Alerted;

        while (Vector3.Distance(transform.position, torchPosition) > 2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, torchPosition, moveSpeed / 50 * Time.deltaTime);
            faceDirection = (torchPosition - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, faceDirection, rotateSpeed * Time.deltaTime);
            yield return null;
        }

        // Toggle the torch on
        torchComponent.toggle();

        yield return new WaitForSeconds(noiseAttentionTime);

        // Move back to the initial position
        yield return ReturnToInitialPosition();
    }
}
