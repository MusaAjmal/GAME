using System.Collections;
using UnityEngine;

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
    public bool noiseDetected = false;
    [SerializeField] private float noiseAttentionTime = 2f;
    private GameObject throwableObject;

    private enum EnemyState
    {
        Patrolling,
        Alerted
    }
    private EnemyState currentState;

    private Vector3 patrolReturnPosition;

    private void Start()
    {
        currentState = EnemyState.Patrolling;
        targetPoint = 0;
        patrolReturnPosition = transform.position;
    }

    private void Update()
    {
        //CheckNoise();

        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
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
                Debug.Log("Chasing");
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

    private void CheckNoise()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, noiseRadius, objectMask);
        if (rangeChecks.Length != 0)
        {
            throwableObject = rangeChecks[0].gameObject;
            Transform target = rangeChecks[0].transform;
            Debug.Log("Noise detected at: " + target.position);

            if (!noiseDetected)
            {
                noiseDetected = true;
                patrolReturnPosition = transform.position;
                StopAllCoroutines();
                StartCoroutine(MoveToNoise(target.position));
            }
        }
    }

    private IEnumerator MoveToNoise(Vector3 noisePosition)
    {
        currentState = EnemyState.Alerted;

        while (Vector3.Distance(transform.position, noisePosition) > 0.1f)
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

        noiseDetected = false;
        Destroy(throwableObject);
        currentState = EnemyState.Patrolling;
    }
}
