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
    public SphereCollider collider; 
    public bool checkpointReached;
    public Vector3 lastpoint;
    private void Start()
    {
        checkpointReached = false;
        collider = GetComponent<SphereCollider>();
    }
    public void OnTriggerEnter(Collider other)
    {
        Respawn.instance.transform.position = transform.position;
        Debug.Log("Checkpoint Added !");
        collider.enabled = false;
        checkpointReached = true;   

    }
}
