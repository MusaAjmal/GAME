using UnityEngine;

public class Stone : MonoBehaviour
{


    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log("Collison Happened!");
        Debug.Log(collision.gameObject.layer);

        if (collision.gameObject.CompareTag("Stone"))
        {
            Debug.Log("Works");
            Vector3 noisePosition = collision.contacts[0].point;
            Enemy[] enemyControllers = FindObjectsOfType<Enemy>();
            foreach (Enemy enemyController in enemyControllers)
            {
                enemyController.CheckDistraction(noisePosition, gameObject);
            }
            Destroy(collision.gameObject);
        }
    }
}
