using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTest : MonoBehaviour
{
    /* public GameObject[] points; // Array of points to light up
     public Material unlitMaterial; // Material for unlit state
     public Material litMaterial; // Material for lit state
     public float lightDuration = 1.0f; // Duration each point stays lit
     public bool loopPath = true; // Whether to loop through the points repeatedly
     public float lightIntensity = 2.0f; // Intensity of point lights (if using lights)

     private int currentPointIndex = 0;
     private Coroutine lightingCoroutine;

     void Start()
     {
         // Initialize all points with the unlit material
         foreach (GameObject point in points)
         {
             SetMaterial(point, unlitMaterial);
             AddPointLight(point);
         }

         // Start lighting up the points
         lightingCoroutine = StartCoroutine(LightUpPath());
     }

     void SetMaterial(GameObject point, Material material)
     {
         MeshRenderer renderer = point.GetComponent<MeshRenderer>();
         if (renderer != null)
         {
             renderer.material = material;
         }
     }

     void AddPointLight(GameObject point)
     {
         if (point.GetComponent<Light>() == null)
         {
             Light pointLight = point.AddComponent<Light>();
             pointLight.type = LightType.Point;
             pointLight.range = 5.0f;
             pointLight.intensity = 0.0f; // Start with no light
         }
     }

     IEnumerator LightUpPath()
     {
         while (true)
         {
             if (points.Length == 0)
                 yield break;

             // Light up the current point
             GameObject currentPoint = points[currentPointIndex];
             SetMaterial(currentPoint, litMaterial);
             Light currentLight = currentPoint.GetComponent<Light>();
             if (currentLight != null)
             {
                 currentLight.intensity = lightIntensity; // Turn on light
             }

             // Wait for the specified duration
             yield return new WaitForSeconds(lightDuration);

             // Unlight the current point
             SetMaterial(currentPoint, unlitMaterial);
             if (currentLight != null)
             {
                 currentLight.intensity = 0.0f; // Turn off light
             }

             // Move to the next point
             currentPointIndex++;
             if (currentPointIndex >= points.Length)
             {
                 if (loopPath)
                 {
                     currentPointIndex = 0;
                 }
                 else
                 {
                     yield break;
                 }
             }
         }
     }

     // Optional method to stop lighting up the path
     public void StopLighting()
     {
         if (lightingCoroutine != null)
         {
             StopCoroutine(lightingCoroutine);
             ResetAllPoints();
         }
     }

     // Reset all points to unlit state
     void ResetAllPoints()
     {
         foreach (GameObject point in points)
         {
             SetMaterial(point, unlitMaterial);
             Light light = point.GetComponent<Light>();
             if (light != null)
             {
                 light.intensity = 0.0f;
             }
         }
     }*/


    /////////////////////////////part 2
    ///
    public GameObject[] points;        // Array of points to light up
    public Material unlitMaterial;     // Material for unlit state
    public Material litMaterial;       // Material for lit state
    public float lightIntensity = 2.0f; // Intensity of point lights (if using lights)
    public float proximityThreshold = 2.0f; // Distance at which the player lights up all points

    private bool allPointsLit = false; // Flag to track if all points are lit
    public static LightTest instance { get; private set; }
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        // Initialize all points with the unlit material
        foreach (GameObject point in points)
        {
            SetMaterial(point, unlitMaterial);
            AddPointLight(point);
        }
    }

    void Update()
    {
        // Check if the player is close to the first point
        if (!allPointsLit && Vector3.Distance(Player.Instance.transform.position, points[0].transform.position) <= proximityThreshold)
        {
            LightUpAllPoints();
            allPointsLit = true; // Set the flag to true so it only lights up once
        }
    }

    void SetMaterial(GameObject point, Material material)
    {
        MeshRenderer renderer = point.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }
    }

    void AddPointLight(GameObject point)
    {
        if (point.GetComponent<Light>() == null)
        {
            Light pointLight = point.AddComponent<Light>();
            pointLight.type = LightType.Point;
            pointLight.range = 5.0f;
            pointLight.intensity = 0.0f; // Start with no light
        }
    }

    void LightUpAllPoints()
    {
        foreach (GameObject point in points)
        {
            // Change material to lit state
            SetMaterial(point, litMaterial);

            // Turn on light if it exists
            Light light = point.GetComponent<Light>();
            if (light != null)
            {
                light.intensity = lightIntensity;
            }
        }
    }
   public void lightPoint(GameObject point)
    {
        SetMaterial(point, litMaterial);
        Light light = point.GetComponent<Light>();
        if (light != null)
        {
            light.intensity = lightIntensity;
        }
    }
}
