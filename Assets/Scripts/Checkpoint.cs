using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    /* public static Checkpoint instance { get; private set; }
     private void Awake()
     {
         instance = this;
     }*/
    public bool checkpointReached;
    public Vector3 lastpoint;
    private void Start()
    {
        checkpointReached = false;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            checkpointReached = true;
            lastpoint = transform.position;

        }
    }
}
