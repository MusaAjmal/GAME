using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] public float radius;
    [Range(0f, 360f)]
    [SerializeField] public float angle;
     public GameObject playerRef;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstructionMask;
    float timeElapsed;
    [SerializeField] private float timeToCheck;

    public bool canseePlayer;
    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }
   /* private void Update()
    {
        StartCoroutine(CheckBoolForTime());
    }
*/
    /*private IEnumerator CheckBoolForTime()
    {
        timeElapsed = 0f;

        while (canseePlayer && timeElapsed < timeToCheck)
        {
            timeElapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        if (canseePlayer && timeElapsed >= timeToCheck)
        {
            Debug.Log("GAME OVER !!! Defeated by " + name);
            return;
            ///break;
*//*            LevelManager.Instance.GameOverScreen();
*//*

            //timeElapsed = 0f;
        }
    }*/



    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }
    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directiontoTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directiontoTarget) < angle / 2)
            {
                float distancetoTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directiontoTarget, distancetoTarget, obstructionMask))
                {
                    canseePlayer = true;
                }
                else
                {
                    canseePlayer = false;
                }
            }
            else
            {
                canseePlayer = false;
            }
        }
        else if (canseePlayer) {
        
         canseePlayer =false;
        }

    }
}
