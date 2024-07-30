using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : MonoBehaviour
{
    [SerializeField] private GameObject points;
    [SerializeField] private GameObject Slingshot;
    [SerializeField] public static bool isSlingshotActive;
    [SerializeField] public static bool isPointActive;


    private void Start()
    {
        isSlingshotActive = false;
        isPointActive = true;
    }
   
    public void onClick()
    {
        isPointActive = false;
        if (isSlingshotActive)
        {
            isSlingshotActive = false;
            isPointActive = true;
          
        }
        else
        {
            isSlingshotActive=true;
        }
        

    }
}
