using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMenu : MonoBehaviour
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
