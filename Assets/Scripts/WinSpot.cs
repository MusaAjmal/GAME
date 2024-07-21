using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinSpot : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Debug.Log("player has won the Game!");
        }
    }
}
