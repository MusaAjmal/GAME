using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    public void set()
    {
        gameObject.SetActive(true);
    }
    public void unset()
    {

    gameObject.SetActive(false); 
    }
}

