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
        Alerted,
        Returning
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
        if (currentState == EnemyState.Idle)
        {
            CheckTorch();

            if ((checkpoint1 != null && checkpoint1.checkpointReached) || (checkpoint2 != null && checkpoint2.checkpointReached))
            {
                RestartCheckPlayerCoroutine();
            }
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
        return currentMovementCoroutine != null;
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
                            StopCurrentMovementCoroutine();
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
        currentState = EnemyState.Alerted;
        float playerDistance = Vector3.Distance(transform.position, playerObject.transform.position);

        if (playerDistance < noiseRadius)
        {
            if (!Physics.Raycast(transform.position, (playerObject.transform.position - transform.position).normalized, playerDistance, obstructionMask))
            {
                Vector3 noisePosition = playerObject.transform.position;

                while (Vector3.Distance(transform.position, noisePosition) > 4f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, noisePosition, moveSpeed * Time.deltaTime);
                    transform.forward = Vector3.Slerp(transform.forward, (noisePosition - transform.position).normalized, rotateSpeed * Time.deltaTime);
                    yield return null;

                    // Break the loop if the enemy detects an obstruction
                    if (Physics.Raycast(transform.position, (playerObject.transform.position - transform.position).normalized, out RaycastHit hit, playerDistance, obstructionMask))
                    {
                        yield break; // Exit the coroutine immediately
                    }
                }

                // Check if the enemy is close enough to catch the player
                if (Vector3.Distance(transform.position, noisePosition) < 4f)
                {
                    LevelManager.Instance.GameOverScreen();
                    StopAllCoroutines();
                    yield break; // Exit the coroutine to prevent stack overflow
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



    public IEnumerator MoveToNoise(Vector3 noisePosition, GameObject throwableObject)
    {
        currentState = EnemyState.Alerted;

        while (Vector3.Distance(transform.position, noisePosition) > 2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, noisePosition, moveSpeed / movementMultiplier * Time.deltaTime);
            transform.forward = Vector3.Slerp(transform.forward, (noisePosition - transform.position).normalized, rotateSpeed * Time.deltaTime);

            // Check for torches while moving to noise
            CheckTorch();

            yield return null;
        }

        yield return new WaitForSeconds(noiseAttentionTime);

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
                StopCurrentMovementCoroutine();
                currentMovementCoroutine = StartCoroutine(MoveToNoise(targetPosition, targetObject));
            }
        }
    }

    private void CheckTorch()
    {
        foreach (var torch in torches)
        {
            if (torch == null)
            {
                Debug.LogWarning("Torch not found");
                return;
            }

            Items torchComponent = torch.GetComponent<Items>();

            if (torchComponent == null)
            {
                Debug.LogWarning("Torch component not found");
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
            StopCurrentMovementCoroutine();
            currentMovementCoroutine = StartCoroutine(MoveToTorch(torchPosition, torchComponent));
        }
    }

    private IEnumerator MoveToTorch(Vector3 torchPosition, Items torchComponent)
    {
        currentState = EnemyState.Alerted;

        // Move towards the torch
        while (Vector3.Distance(transform.position, torchPosition) > 2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, torchPosition, (moveSpeed /*/ torchMultiplier*/) * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((torchPosition - transform.position).normalized), rotateSpeed * 5 * Time.deltaTime);
            yield return null;
        }

        // Toggle the torch if it's within reach
        if (torchComponent != null && Vector3.Distance(transform.position, torchPosition) <= 2f)
        {
            torchComponent.toggle();
        }
        else
        {
            Debug.LogWarning("Torch component not found or not within reach.");
        }

        // Wait for a moment to simulate attention on the torch
        yield return new WaitForSeconds(noiseAttentionTime);

        // Return to initial position
        yield return ReturnToInitialPosition();
    }

    private IEnumerator ReturnToInitialPosition()
    {
        currentState = EnemyState.Returning;

        while (Vector3.Distance(transform.position, initialPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(initialPosition - transform.position), rotateSpeed * Time.deltaTime);
            yield return null;
        }

        // Reset rotation
        transform.rotation = initialRotation;

        // Reset state
        ResetToInitialState();
    }

    private void ResetToInitialState()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        currentState = EnemyState.Idle;
        StartCheckPlayerCoroutine();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with " + collision.gameObject.name);
    }
}
