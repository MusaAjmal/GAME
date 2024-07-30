using UnityEngine;

public class Stone : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Happened!" + gameObject.tag);
        Debug.Log(collision.gameObject.layer);

        if (collision.gameObject.CompareTag("Stone"))
        {
            Debug.Log("Works");
            Vector3 noisePosition = collision.contacts[0].point;
            Enemy[] enemyControllers = FindObjectsOfType<Enemy>();
            BigEnemy[] bigEnemies = FindObjectsOfType<BigEnemy>();

            // Combine both types of enemies into a single collection
            System.Collections.Generic.List<MonoBehaviour> allEnemies = new System.Collections.Generic.List<MonoBehaviour>();
            allEnemies.AddRange(enemyControllers);
            allEnemies.AddRange(bigEnemies);

            // Find the nearest enemy
            MonoBehaviour nearestEnemy = FindNearestEnemy(allEnemies, noisePosition);
            if (nearestEnemy is Enemy)
            {
                (nearestEnemy as Enemy).CheckDistraction(noisePosition, gameObject);
            }
            else if (nearestEnemy is BigEnemy)
            {
                (nearestEnemy as BigEnemy).CheckDistraction(noisePosition, gameObject);
            }

            // Destroy the stone object after the collision
            Destroy(collision.gameObject);
        }
    }

    private MonoBehaviour FindNearestEnemy(System.Collections.Generic.List<MonoBehaviour> enemies, Vector3 position)
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
