using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void unset()
    {
       gameObject.SetActive(false);
    }
    public void set()
    {
        gameObject.SetActive(true);
    }
}
