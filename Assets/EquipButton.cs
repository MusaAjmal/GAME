using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipButton : MonoBehaviour
{
    public delegate void EquipbuttonPressed();
    public EquipbuttonPressed BTCallback;
    public static EquipButton Instance {  get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public void buttonPressed()
    {
       BTCallback?.Invoke();
        Chest.Instance.Interact();
    }
    
}
