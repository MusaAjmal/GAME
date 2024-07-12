using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Diagnostics;

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
    private enum EnemyState
    {
        Patrolling,
        Alerted
    }
    private EnemyState currentState;

    private void Start()
    {
        currentState = EnemyState.Patrolling;
        targetPoint = 0;    
    }

    private void Update()
    {
        ChasePlayer();
        switch (currentState) {

            case EnemyState.Patrolling:
                Patrol();
                break;

            case EnemyState.Alerted:

                break;

        }
           
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

        if (isMoving())
        {
            lastInteractDirection = transform.position;
            transform.forward = Vector3.Slerp(transform.forward, faceDirection, rotateSpeed * Time.deltaTime);
        }

    }
    
    private void ChasePlayer()
    {
        if (Physics.Raycast(transform.position,transform.forward,out RaycastHit raycastHit,lookDistance))
        {
            if (raycastHit.transform.TryGetComponent(out Player player)) {
                Debug.Log("Chasing");
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
    private bool isMoving()
    {
        return faceDirection != Vector3.zero;
    }
}
