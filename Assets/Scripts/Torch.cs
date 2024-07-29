using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] public bool IsToggledOn;

    public void Start()
    {
        IsToggledOn = false;
    }

    public void Toggle(bool value)
    {
        IsToggledOn = value;
    }

}
