using System.Collections;
using UnityEngine;

public class Dog : MonoBehaviour
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
    public bool noiseDetected = false;
    [SerializeField] private float noiseAttentionTime = 2f;
    private GameObject throwableObject;
    [SerializeField] private float playerDetectDistance;
    private bool spotted;

    private enum EnemyState
    {
        Patrolling,
        Alerted
    }
    private EnemyState currentState;

    private Vector3 patrolReturnPosition;

    private void Start()
    {
        spotted = false;    
        currentState = EnemyState.Patrolling;
        targetPoint = 0;
        patrolReturnPosition = transform.position;
    }

    private void Update()
    {
        CheckNoise();
        CheckPlayer();

        switch (currentState)
        {
            case EnemyState.Patrolling:
                if (!spotted) {
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

    public void CheckNoise()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, noiseRadius, objectMask);

        if (rangeChecks.Length != 0)
        {

            GameObject targetObject = null;
            Vector3 targetPosition = Vector3.zero;

            foreach (Collider collider in rangeChecks)
            {

               
                if (collider.gameObject.layer == LayerMask.NameToLayer("Object")) // Replace "YourLayerName" with the actual layer name
                {
                    //Debug.Log("Noise Detected");
                    // Check if there is no obstruction between the current object and the detected object
                    Vector3 directionToTarget = collider.transform.position - transform.position;
                    float distanceToTarget = directionToTarget.magnitude;
                    directionToTarget.Normalize();

                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    {
                        //Debug.Log("Object in range !!!!");
                        targetObject = collider.gameObject;
                        targetPosition = collider.transform.position;
                        break; // Found the target, no need to continue the loop
                    }
                }
            }

            if (targetObject != null && targetObject.tag == "Bone")
            {
                throwableObject = targetObject;
/*                Debug.Log("Noise detected at: " + targetPosition + " from " + targetObject.tag);
*/                // Check if the target position is below the current object's position
                if (targetPosition.y < transform.position.y)
                {



                    if (patrolReturnPosition == Vector3.zero)
                    {
                        patrolReturnPosition = transform.position;
/*                        Debug.Log("Setting patrolReturnPosition to: " + patrolReturnPosition);
*/                    }


                    StopAllCoroutines();
                    StartCoroutine(MoveToNoise(targetPosition, throwableObject));
                }
            }
            else
            {
/*                Debug.Log("No unobstructed noise source found.");
*/            }
        }
        else
        {
/*            Debug.Log("No noise detected.");
*/        }
    }

    public void CheckPlayer()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, playerDetectDistance);
        Debug.Log(rangeChecks);

        if (rangeChecks.Length != 0)
        {
            GameObject targetObject = null;
            Vector3 targetPosition = Vector3.zero;

            foreach (Collider collider in rangeChecks)
            {
                if (collider.CompareTag("Player"))
                {
                    Debug.Log("Player found");
                    targetObject = collider.gameObject;
                    targetPosition = collider.transform.position;
                    float distanceToTarget = Vector3.Distance(targetPosition, transform.position);

                    if (!Physics.Raycast(transform.position, targetPosition - transform.position, distanceToTarget, obstructionMask))
                    {
                        // Calculate the direction to the target
                        Vector3 directionToTarget = (targetPosition - transform.position).normalized;

                        // Calculate the desired rotation
                        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

                        // Smoothly rotate towards the target rotation
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

                        spotted = true;
                        Debug.Log("GAME OVER ENEMY SPOTTED YOU");
                        break; // Found the target, no need to continue the loop
                    }
                }
            }
        }
    }


    public IEnumerator MoveToNoise(Vector3 noisePosition , GameObject throwableObject)
    {
        currentState = EnemyState.Alerted;


        while (Vector3.Distance(transform.position, noisePosition) > 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, noisePosition, moveSpeed * Time.deltaTime);
            faceDirection = (noisePosition - transform.position).normalized;
            transform.forward = Vector3.Slerp(transform.forward, faceDirection, rotateSpeed * Time.deltaTime);
            yield return null;
        }

/*        yield return new WaitForSeconds(2f);
*/        
        Destroy(throwableObject);

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


  
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("alooooooooooooooooooo");
    }

    public static bool ObjectIsInLayerMask(GameObject obj, LayerMask mask) => (mask.value & (1 << obj.layer)) != 0;

}
