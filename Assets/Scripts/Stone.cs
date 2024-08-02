using UnityEngine;
using System.Collections.Generic;

public class Stone : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Ensure the collision is only processed if this stone is the one involved in the collision
        if (collision.gameObject.CompareTag("Stone"))
        {
            Debug.Log("Collision Happened!" + gameObject.tag + gameObject.name);
            Debug.Log(collision.gameObject.layer);

            // Vector3 noisePosition = collision.gameObject.transform.position; // Get the location of the collision
            Vector3 noisePosition = collision.contacts[0].point; // Get the location of the collision
            Debug.Log("Collision Point: " + noisePosition);

            Enemy[] enemyControllers = FindObjectsOfType<Enemy>();
            BigEnemy[] bigEnemies = FindObjectsOfType<BigEnemy>();

            // Combine both types of enemies into a single collection
            List<MonoBehaviour> allEnemies = new List<MonoBehaviour>();
            allEnemies.AddRange(enemyControllers);
            allEnemies.AddRange(bigEnemies);

            // Find the nearest enemy
            MonoBehaviour nearestEnemy = FindNearestEnemy(allEnemies, noisePosition);
            if (nearestEnemy is Enemy)
            {
                (nearestEnemy as Enemy).CheckDistraction(noisePosition,gameObject);
            }
            else if (nearestEnemy is BigEnemy)
            {
                (nearestEnemy as BigEnemy).CheckDistraction(noisePosition,gameObject);
            }

            // Destroy the stone object after the collision
            // Destroy(gameObject);
        }
    }

    private MonoBehaviour FindNearestEnemy(List<MonoBehaviour> enemies, Vector3 position)
    {
        MonoBehaviour nearestEnemy = null;
        float smallestDistance = float.MaxValue;

        foreach (MonoBehaviour enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, position);
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
}