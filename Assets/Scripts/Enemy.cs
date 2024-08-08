using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject[] movePoints;
    [SerializeField] private float moveSpeed;
    private Vector3 faceDirection;
    private int targetPoint;
    private float constantY = 1.07f;
    private bool isIncreasing;
    private float rotateSpeed = 5f;
    private float lookDistance = 100f;
    private Vector3 lastInteractDirection;

    [SerializeField] public float noiseRadius;
    [SerializeField] private LayerMask objectMask;
    [SerializeField] private LayerMask obstructionMask;
    [SerializeField] private LayerMask distractionMask;
    [SerializeField] private float collisionRange = 10f;
    [SerializeField] private EnemyVision enemyVision;
    public bool noiseDetected = false;
    [SerializeField] private float noiseAttentionTime = 2f;
    private GameObject throwableObject;
    [Range(0f, 100f)]
    [SerializeField] private float playerDetectDistance;
    private bool spotted;
    private LevelManager sceneManager;
    private bool isChasingPlayer; // To track if the coroutine is already running
    [SerializeField] private Checkpoint Checkpoint;
    [SerializeField] private Checkpoint Checkpoint2;
    private enum EnemyState
    {
        Patrolling,
        Alerted
    }

    private EnemyState currentState;

    private Vector3 patrolReturnPosition;

    private void Start()
    {
        sceneManager = LevelManager.Instance;
        spotted = false;
        currentState = EnemyState.Patrolling;
        targetPoint = 0;
        patrolReturnPosition = transform.position;
    }

    private void Update()
    {
        if (!isChasingPlayer) // Only check if not already chasing
        {
            StartCoroutine(CheckPlayer());
        }

        switch (currentState)
        {
            case EnemyState.Patrolling:
                if (Checkpoint != null && Checkpoint.checkpointReached && spotted || Checkpoint2!=null && Checkpoint2.checkpointReached && spotted)
                {
                    Patrol();
                   
                    CheckPlayer();
                }
                if (!spotted)
                {
                    Patrol();
                }
                break;

            case EnemyState.Alerted:
                // Handle any specific behavior for the Alerted state
                break;
        }

        // ChasePlayer();
    }

    private void Patrol()
    {
        Vector3 targetPosition = new Vector3(movePoints[targetPoint].transform.position.x, constantY, movePoints[targetPoint].transform.position.z);
        Vector3 currentPosition = new Vector3(transform.position.x, constantY, transform.position.z);

        if (transform.position == targetPosition)
        {
            if (isIncreasing)
            {
                IncreaseTarget();
            }
            else
            {
                DecreaseTarget();
            }
        }

        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);
        faceDirection = (targetPosition - transform.position).normalized;

        if (IsMoving())
        {
            lastInteractDirection = transform.position;
            transform.forward = Vector3.Slerp(transform.forward, faceDirection, rotateSpeed * Time.deltaTime);
        }
    }

    private void ChasePlayer()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit raycastHit, lookDistance))
        {
            if (raycastHit.transform.TryGetComponent(out Player player))
            {
                // Debug.Log("Chasing");
                // Implement chasing logic here
            }
        }
    }

    private void IncreaseTarget()
    {
        targetPoint++;
        if (targetPoint >= movePoints.Length)
        {
            targetPoint = movePoints.Length - 1;
            isIncreasing = false;
        }
    }

    private void DecreaseTarget()
    {
        targetPoint--;
        if (targetPoint < 0)
        {
            targetPoint = 0;
            isIncreasing = true;
        }
    }

    private bool IsMoving()
    {
        return faceDirection != Vector3.zero;
    }

   


    public IEnumerator CheckPlayer()
    {
        // Check for colliders within the player detection radius
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, playerDetectDistance);

        if (rangeChecks.Length != 0)
        {
            GameObject targetObject = null;
            Vector3 targetPosition = Vector3.zero;

            foreach (Collider collider in rangeChecks)
            {
                if (collider.CompareTag("Player")) // Check if the collider is tagged as Player
                {
                    Debug.Log("Player found");
                    targetObject = collider.gameObject;
                    targetPosition = collider.transform.position;
                    float distanceToTarget = Vector3.Distance(targetPosition, transform.position);

                    // Ensure there is no obstruction between the enemy and the player
                    if (!Physics.Raycast(transform.position, targetPosition - transform.position, distanceToTarget, obstructionMask))
                    {
                        // Set the spotted flag and chasing state
                        spotted = true;
                        isChasingPlayer = true;

                        // Calculate the direction to the player
                        Vector3 directionToTarget = (targetPosition - transform.position).normalized;

                        // Calculate the desired rotation towards the player
                        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

                        // Smoothly rotate towards the player
                        float elapsedTime = 0f; // Initialize the elapsed time
                        float timeToRotate = 1f; // Duration to complete the rotation

                        // Rotate towards the player for the specified duration
                        while (elapsedTime < timeToRotate)
                        {
                            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, elapsedTime / timeToRotate);
                            elapsedTime += Time.deltaTime;
                            yield return null; // Wait for the next frame
                        }

                        // Wait for an additional 1 second before ending the game
                        yield return new WaitForSeconds(1f);

                        // Trigger the Game Over screen

                        sceneManager.GameOverScreen();
                        Debug.Log("GAME OVER: ENEMY SPOTTED YOU");
                        break; // Exit the loop once the player is spotted
                    }
                }
            }
        }

        // Reset chasing state after the detection loop
        isChasingPlayer = false;
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

        while (Vector3.Distance(transform.position, patrolReturnPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, patrolReturnPosition, moveSpeed * Time.deltaTime);
            faceDirection = (patrolReturnPosition - transform.position).normalized;
            transform.forward = Vector3.Slerp(transform.forward, faceDirection, rotateSpeed * Time.deltaTime);
            yield return null;
        }

        currentState = EnemyState.Patrolling;
        patrolReturnPosition = Vector3.zero;
    }

    public void CheckDistraction(Vector3 noisePosition, GameObject throwableObject)
    {

        // Collider array to hold all colliders within the noiseRadius and on the distractionMask layer
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, noiseRadius, distractionMask);

        // Check if any colliders were found
        if (rangeChecks.Length != 0)
        {
            GameObject targetObject = null;
            Vector3 targetPosition = Vector3.zero;

            foreach (Collider collider in rangeChecks)
            {
                // Debug.Log("Object found in range: " + collider.gameObject.name);

                // Check if the collider's gameObject is in the Distraction layer
                if (collider.gameObject.layer == LayerMask.NameToLayer("Distraction"))
                {
                    Vector3 directionToTarget = noisePosition - transform.position;
                    float distanceToTarget = directionToTarget.magnitude;
                    directionToTarget.Normalize();

                    // Check if there's no obstruction between the enemy and the target
                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    {
                        targetObject = throwableObject;
                        targetPosition = throwableObject.transform.position;
                        break;
                    }
                    else
                    {
                        // Debug.Log("Obstruction found between enemy and distraction: " + collider.gameObject.name);
                    }
                }
                else
                {
                    //  Debug.Log("Object not in Distraction layer: " + collider.gameObject.name);
                }
            }

            if (targetObject != null)
            {
                // Debug.Log("Distraction detected at: " + targetPosition);

                if (patrolReturnPosition == Vector3.zero)
                {
                    patrolReturnPosition = transform.position;
                    // Debug.Log("Setting patrolReturnPosition to: " + patrolReturnPosition);
                }

                StopAllCoroutines();
                StartCoroutine(MoveToNoise(targetPosition, targetObject));
            }
            else
            {
                // Debug.Log("No valid distraction target found.");
            }
        }
        else
        {
            // Debug.Log("No objects found in range.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("alooooooooooooooooooo");
    }

    public static bool ObjectIsInLayerMask(GameObject obj, LayerMask mask) => (mask.value & (1 << obj.layer)) != 0;
}
