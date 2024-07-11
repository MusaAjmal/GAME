using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Diagnostics;

public class Enemy : MonoBehaviour
{
    private bool isEastWest = false;
    private bool isNorthSouth = false;
    private float moveTowards = 3f;
    private float lookDistance = 10f;
    private Vector3 chasePosition;
    private enum State
    {
        Alerted,
        Patrolling
    }
    private State state;
    private Vector3 previousPosition;
    private Vector3 targetForward;
    private Vector3 origin;
    private void Start()
    {
       state  = State.Patrolling;
        chasePosition = Player.Instance.GetPosition();
        origin = transform.position;
        if (Math.Abs(transform.position.z) > 0 && Math.Abs(transform.position.x) < 1)
        {
            isNorthSouth = true;
        }
        if (Math.Abs(transform.position.x) > Math.Abs(transform.position.z))
        {

            isEastWest = true;
        }
        StartCoroutine("MovePlayerForward");
       // StartCoroutine("ChasePlayer");
    }
    private void Update()
    {
       
    }
    private IEnumerator MovePlayerForward()
    {
        
        
            while (true)
            {
                if (isNorthSouth)
                {
                    if (transform.position.z > 0)
                    { //north
                        targetForward = new Vector3(transform.position.x, 1.07f, transform.position.z - moveTowards);

                        while (Vector3.Distance(transform.position, targetForward) > 0.01f)
                        {

                            transform.position = Vector3.MoveTowards(transform.position, targetForward, Time.deltaTime);
                            yield return null;
                        }

                    }
                    if (transform.position.z < 0) //south
                    {
                        targetForward = new Vector3(transform.position.x, 1.07f, transform.position.z + moveTowards);

                        while (Vector3.Distance(transform.position, targetForward) > 0.01f)
                        {

                            transform.position = Vector3.MoveTowards(transform.position, targetForward, Time.deltaTime);
                            yield return null;
                        }
                    }

                    if (Vector3.Distance(transform.position, targetForward) < 0.01f)  //leftMovement
                    {
                        previousPosition = targetForward;
                        targetForward = new Vector3(transform.position.x - moveTowards, 1.07f, transform.position.z);
                        while (Vector3.Distance(transform.position, targetForward) > 0.01f)
                        {

                            transform.position = Vector3.MoveTowards(transform.position, targetForward, Time.deltaTime);
                            yield return null;
                        }

                    }



                    if (Vector3.Distance(transform.position, targetForward) < 0.1f)
                    {

                        while (Vector3.Distance(transform.position, previousPosition) > 0.01f)
                        {

                            transform.position = Vector3.MoveTowards(transform.position, previousPosition, Time.deltaTime);
                            yield return null;
                        }

                    }
                    if (Vector3.Distance(transform.position, previousPosition) < 0.1f)
                    {

                        while (Vector3.Distance(transform.position, origin) > 0.01f)
                        {

                            transform.position = Vector3.MoveTowards(transform.position, origin, Time.deltaTime);
                            yield return null;
                        }

                    }


                }
                if (isEastWest)
                {
                    if (transform.position.x > 0)
                    {
                        targetForward = new Vector3(transform.position.x - moveTowards, 1.07f, transform.position.z);

                        while (Vector3.Distance(transform.position, targetForward) > 0.01f)
                        {

                            transform.position = Vector3.MoveTowards(transform.position, targetForward, Time.deltaTime);
                            yield return null;
                        }
                    }
                    else
                    {

                        targetForward = new Vector3(transform.position.x + moveTowards, 1.07f, transform.position.z);

                        while (Vector3.Distance(transform.position, targetForward) > 0.01f)
                        {

                            transform.position = Vector3.MoveTowards(transform.position, targetForward, Time.deltaTime);
                            yield return null;
                        }
                    }


                    if (Vector3.Distance(transform.position,targetForward) < 0.01f)  //leftMovement
                    {
                        previousPosition = targetForward;
                        targetForward = new Vector3(transform.position.x, 1.07f, transform.position.z + moveTowards);
                        while (Vector3.Distance(transform.position, targetForward) > 0.01f)
                        {

                            transform.position = Vector3.MoveTowards(transform.position, targetForward, Time.deltaTime);
                            yield return null;
                        }

                    }
                    if (Vector3.Distance(transform.position, targetForward) < 0.1f)
                    {

                        while (Vector3.Distance(transform.position, previousPosition) > 0.01f)
                        {

                            transform.position = Vector3.MoveTowards(transform.position, previousPosition, Time.deltaTime);
                            yield return null;
                        }

                    }



                }
                if (Vector3.Distance(transform.position, previousPosition) < 0.1f)
                {

                    while (Vector3.Distance(transform.position, origin) > 0.01f)
                    {

                        transform.position = Vector3.MoveTowards(transform.position, origin, Time.deltaTime);
                        yield return null;
                    }

                }

            }
        
    }

    private IEnumerator ChasePlayer()
    {
        while (true)
        {//hi
            if (Vector3.Distance(transform.position, chasePosition) < lookDistance)
            {
                StopCoroutine("MovePlayerForward");

                transform.position = Vector3.MoveTowards(transform.position, chasePosition, Time.deltaTime);   
            }
            else
            {
                StartCoroutine("MovePlayerForward");
            }
            
            yield return null;
        }
    }

}
