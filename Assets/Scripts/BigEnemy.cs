using System.Collections;
using UnityEngine;

public class BigEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotateSpeed = 2f;

    [Header("Detection Settings")]
    [SerializeField] private float lookDistance = 100f;
    [SerializeField] private float noiseRadius = 6f;
    [SerializeField] private float playerDetectDistance = 20f;
    [SerializeField] private float torchCheckRange = 10f;

    [Header("Timing Settings")]
    [Range(0f, 100f)]
    [SerializeField] private float noiseAttentionTime = 2f;

    [Header("Multipliers")]
    [SerializeField] private float movementMultiplier = 1f;
    [SerializeField] private float torchMultiplier = 50f;
    [SerializeField] private float noiseMultiplier = 1f;

    [Header("Layers")]
    [SerializeField] private LayerMask objectMask;
    [SerializeField] private LayerMask obstructionMask;
    [SerializeField] private LayerMask distractionMask;

    [Header("References")]
    [SerializeField] private Checkpoint checkpoint1;
    [SerializeField] private Checkpoint checkpoint2;
    [SerializeField] private Items[] torches;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private Coroutine playerCheckCoroutine;
    private Coroutine currentMovementCoroutine;

    private enum EnemyState
    {
        Idle,
        Alerted
    }

    private EnemyState currentState;

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        currentState = EnemyState.Idle;
        StartCheckPlayerCoroutine();
    }

    private void Update()
    {
        CheckTorch();

        if ((checkpoint1 != null && checkpoint1.checkpointReached) || (checkpoint2 != null && checkpoint2.checkpointReached))
        {
            RestartCheckPlayerCoroutine();
        }
    }

    private void StartCheckPlayerCoroutine()
    {
        if (playerCheckCoroutine == null)
        {
            playerCheckCoroutine = StartCoroutine(CheckPlayerCoroutine());
        }
    }

    private void StopCheckPlayerCoroutine()
    {
        if (playerCheckCoroutine != null)
        {
            StopCoroutine(playerCheckCoroutine);
            playerCheckCoroutine = null;
        }
    }

    private void RestartCheckPlayerCoroutine()
    {
        StopCheckPlayerCoroutine();
        StartCheckPlayerCoroutine();
    }

    private bool IsMoving()
    {
        return transform.forward != Vector3.zero;
    }

    private IEnumerator CheckPlayerCoroutine()
    {
        while (true)
        {
            foreach (var item in torches)
            {
                if (item.isActive())
                {
                    Collider[] rangeChecks = Physics.OverlapSphere(transform.position, playerDetectDistance);

                    if (rangeChecks.Length != 0)
                    {
                        GameObject targetObject = null;
                        Vector3 targetPosition = Vector3.zero;

                        foreach (Collider collider in rangeChecks)
                        {
                            if (collider.gameObject.CompareTag("Player"))
                            {
                                Vector3 directionToTarget = (collider.transform.position - transform.position).normalized;
                                float distanceToTarget = Vector3.Distance(transform.position, collider.transform.position);

                                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                                {
                                    targetObject = collider.gameObject;
                                    targetPosition = collider.transform.position;
                                    break;
                                }
                            }
                        }

                        if (targetObject != null)
                        {
                            StopCheckPlayerCoroutine();
                            currentMovementCoroutine = StartCoroutine(MoveToPlayer(targetObject));
                        }
                    }
                }
            }

            yield return null;
        }
    }

    private IEnumerator MoveToPlayer(GameObject playerObject)
    {
        bool playerCaught = false;
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
                    transform.forward = Vector3.Slerp(transform.forward, (noisePosition - transform.position).normalized, rotateSpeed * Time.deltaTime);
                    yield return null;

                    if (Physics.Raycast(transform.position, (playerObject.transform.position - transform.position).normalized, out RaycastHit hit, playerDistance, obstructionMask))
                    {
                        break;
                    }
                }

                if (Vector3.Distance(transform.position, noisePosition) < 2f)
                {
                    playerCaught = true;

                    if (playerCaught)
                    {
                        LevelManager.Instance.GameOverScreen();
                        StopAllCoroutines();
                        transform.position = initialPosition;
                        transform.rotation = initialRotation;
                        currentState = EnemyState.Idle;
                        StartCheckPlayerCoroutine();
                        yield break;
                    }
                }
            }
        }

        yield return ReturnToInitialPosition();
    }

    private void StopCurrentMovementCoroutine()
    {
        if (currentMovementCoroutine != null)
        {
            StopCoroutine(currentMovementCoroutine);
            currentMovementCoroutine = null;
        }
    }

    private IEnumerator ReturnToInitialPosition()
    {
        while (Vector3.Distance(transform.position, initialPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * movementMultiplier * Time.deltaTime);
            transform.forward = Vector3.Slerp(transform.forward, (initialPosition - transform.position).normalized, rotateSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = initialRotation;
        currentState = EnemyState.Idle;
        StartCheckPlayerCoroutine();
    }

    public IEnumerator MoveToNoise(Vector3 noisePosition, GameObject throwableObject)
    {
        currentState = EnemyState.Alerted;

        while (Vector3.Distance(transform.position, noisePosition) > 2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, noisePosition, moveSpeed * Time.deltaTime);
            transform.forward = Vector3.Slerp(transform.forward, (noisePosition - transform.position).normalized, rotateSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(noiseAttentionTime);

        movementMultiplier = noiseMultiplier;

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
                if (collider.gameObject.layer == LayerMask.NameToLayer("Distraction") && collider.gameObject == throwableObject)
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
                }
            }

            if (targetObject != null)
            {
                StopCheckPlayerCoroutine();
                currentMovementCoroutine = StartCoroutine(MoveToNoise(targetPosition, targetObject));
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected");
    }

    private void CheckTorch()
    {
        foreach (var torch in torches)
        {
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
                return;
            }

            Vector3 torchPosition = torch.transform.position;

            if (Vector3.Distance(transform.position, torchPosition) > torchCheckRange)
            {
                return;
            }

            StopCheckPlayerCoroutine();
            currentMovementCoroutine = StartCoroutine(MoveToTorch(torchPosition, torchComponent));
        }
    }

    private IEnumerator MoveToTorch(Vector3 torchPosition, Items torchComponent)
    {
        currentState = EnemyState.Alerted;

        while (Vector3.Distance(transform.position, torchPosition) > 2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, torchPosition, (moveSpeed / torchMultiplier) * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((torchPosition - transform.position).normalized), rotateSpeed * 5 * Time.deltaTime);
            yield return null;
        }

        torchComponent.toggle();
        yield return new WaitForSeconds(noiseAttentionTime);

        movementMultiplier = 0.01f;

        yield return ReturnToInitialPosition();
    }
}
