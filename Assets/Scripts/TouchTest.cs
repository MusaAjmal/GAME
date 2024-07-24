using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTest : MonoBehaviour
{
    
    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3  touchPosition = Camera.main.ScreenToViewportPoint(touch.position);
           // touchPosition.y = 0;
            transform.position = touchPosition; 
        }
    }
}
